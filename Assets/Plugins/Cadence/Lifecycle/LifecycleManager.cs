using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cadence.Lifecycle {
	public class LifecycleManager : MonoBehaviour {
		public List<LifecycleModule> lifecycleObjects = new List<LifecycleModule>();

		public bool LoadOnStart = true;

		private void OnEnable() {
			BuildEventList();
			if(LoadOnStart)
				InitializeDependencyTree();
		}

		private void OnDisable() {
			DecommissionDependencyTree();
			ClearEventList();
		}

		private void FixedUpdate() {
			foreach (LifecycleModule lifecycleObject in lifecycleObjects) {
				try {
					lifecycleObject._FixedUpdate();
				}
				catch (NotImplementedException e) { }
				catch (Exception e) {
					Debug.LogException(e);
				}
			}
		}

		private void Update() {
			foreach (LifecycleModule lifecycleObject in lifecycleObjects) {
				try {
					lifecycleObject._Update();
				}
				catch (NotImplementedException e) { }
				catch (Exception e) {
					Debug.LogException(e);
				}
			}
		}

		private void LateUpdate() {
			foreach (LifecycleModule lifecycleObject in lifecycleObjects) {
				try {
					lifecycleObject._LateUpdate();
				}
				catch (NotImplementedException e) { }
				catch (Exception e) {
					Debug.LogException(e);
				}
			}
		}

		public void InitializeDependencyTree() {
			foreach (LifecycleModule lifecycleObject in lifecycleObjects.Where(lifecycleObject =>
				lifecycleObject._thingsIneed.Count == 0)) {
				lifecycleObject._TryCallEnable(null);
			}
		}

		public void DecommissionDependencyTree() {
			foreach (LifecycleModule lifecycleObject in lifecycleObjects.Where(lifecycleObject =>
				lifecycleObject._thingsThatNeedMe.Count == 0)) {
				lifecycleObject._TryCallDisable(null);
			}
		}

		private void ClearEventList() {
			lifecycleObjects = new List<LifecycleModule>();
		}

		private void BuildEventList() {
			foreach (System.Type type in
				AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()
					.Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(LifecycleModule))))) {
				Debug.Log("LifecycleObject: Found valid class '" + type + "'");
				LifecycleModule de = (LifecycleModule) Activator.CreateInstance(type);
				lifecycleObjects.Add(de);
			}
			foreach (LifecycleModule lifecycleObject in lifecycleObjects) {
				LoadDependencies(lifecycleObject);
			}
		}

		private void LoadDependencies(LifecycleModule obj) {
			object[] attribs = obj.GetType().GetCustomAttributes(true);
			foreach (object attrib in attribs) {
				if (attrib.GetType() != typeof(LifecycleDependency)) continue;
				LifecycleDependency dps = (LifecycleDependency) attrib;
				foreach (Type type in dps._dependencies) {
					if (!type.IsSubclassOf(typeof(LifecycleModule))) {
						Debug.LogError($"Type {type} is not of type {typeof(LifecycleModule).Name}: Skipping.");
						continue;
					}

					LifecycleModule needed = lifecycleObjects.FirstOrDefault((l) =>
						l.GetType() == type
					);
					if (needed == default(LifecycleModule)) {
						Debug.LogError("SCARY ERROR");
						continue;
					}

					if (obj.GetType() == needed.GetType()) continue;
					if (!obj._thingsIneed.Contains(needed))
						obj._thingsIneed.Add(needed);
					if (!needed._thingsThatNeedMe.Contains(obj))
						needed._thingsThatNeedMe.Add(obj);
				}
			}
		}
	}

	[Serializable]
	public abstract class LifecycleModule {
		public bool _initialized = false;
		
		// After 20 minutes of dyslexia fueled rage these are their names now. 
		public List<LifecycleModule> _thingsIneed = new List<LifecycleModule>();
		public List<LifecycleModule> _thingsThatNeedMe = new List<LifecycleModule>();

		#region Initialize

		public UInt32 _dependencyMask;
		public UInt32 _initMask => (1u << _thingsIneed.Count) - 1;

		public void TryEnable() {
			_TryCallEnable();
		}

		public void _TryCallEnable(LifecycleModule d = null) {
			if (_initialized) {
				Debug.LogError($"{GetType()} is already initialized!");
				return;
			}

			if (_thingsIneed.Count == 0) {
				OnEnable();
				return;
			}

			if (d != null) {
				if (!_thingsIneed.Contains(d)) {
					Debug.LogError($"{d.GetType()} is not a dependency");
					return;
				}

				int index = _thingsIneed.IndexOf(d);

				_dependencyMask |= 1u << index;
			}

			if (_dependencyMask == _initMask) {
				OnEnable();
			}
		}

		protected void FinalizeInitialization() {
			if (_initialized) return;
			_initialized = true;
			_dependencyMask = 0;
			for (int i = 0; i < _thingsThatNeedMe.Count; i++) {
				_thingsThatNeedMe[i]._TryCallEnable(this);
			}
		}

		#endregion

		#region Decommission

		public UInt32 _dependentMask;
		public UInt32 _decomMask => (1u << _thingsThatNeedMe.Count) - 1;

		public void _TryCallDisable(LifecycleModule d) {
			if (!_initialized) {
				Debug.LogError($"{GetType()} is already Uninitialized!");
				return;
			}

			if (_thingsThatNeedMe.Count == 0) {
				OnDisable();
				return;
			}

			if (!_thingsThatNeedMe.Contains(d)) {
				Debug.LogError($"{d.GetType()} is not dependent");
				return;
			}

			int index = _thingsThatNeedMe.IndexOf(d);

			_dependentMask |= 1u << index;

			if (_dependentMask == _decomMask) {
				OnDisable();
			}
		}

		protected void FinalizeDecommission() {
			if (!_initialized) return;
			_initialized = false;
			_dependentMask = 0;
			for (int i = 0; i < _thingsIneed.Count; i++) {
				_thingsIneed[i]._TryCallDisable(this);
			}
		}

		#endregion

		protected abstract void OnEnable();
		protected abstract void OnDisable();

		public void _FixedUpdate() { 
			if(!_initialized) return;
			FixedUpdate();
		}
		public virtual void FixedUpdate() { }
		public void _Update() { 
			if(!_initialized) return;
			Update();
		}
		public virtual void Update() { }
		public void _LateUpdate() {
			if(!_initialized) return;
			LateUpdate();
		}
		public virtual void LateUpdate() { }
	}

	[AttributeUsage(AttributeTargets.Class,
		AllowMultiple = false,
		Inherited = true)]
	public class LifecycleDependency : Attribute {
		public Type[] _dependencies { get; }

		public LifecycleDependency(params Type[] dependencies) {
			_dependencies = dependencies;
		}

		public LifecycleDependency(Type dependencies) {
			if (dependencies.IsSubclassOf(typeof(LifecycleModule)))
				_dependencies = new[] {dependencies};
			else
				_dependencies = new Type[0];
		}

		public LifecycleDependency() {
			_dependencies = new Type[0];
		}
	}
}