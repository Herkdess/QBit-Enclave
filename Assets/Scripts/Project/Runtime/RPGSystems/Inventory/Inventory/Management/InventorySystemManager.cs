using System;
using UnityEngine;
namespace RPGSystems {
    public class InventorySystemManager : MonoBehaviour {

        private ItemDatabaseObject[] itemDatabases;

        private UserInterface[] UserInterfaces;

        private void Start() {
            Initialize();
        }

        private void Initialize() {
            RPGControls.Initialize();
            itemDatabases = RPGControls.GetAllDatabases();
            foreach (ItemDatabaseObject itemDatabase in itemDatabases) {
                itemDatabase.Initialize();
            }
            
            UserInterfaces = FindObjectsOfType<UserInterface>();
            foreach (UserInterface userInterface in UserInterfaces) {
                userInterface.Init();
            }
            
            RPGControls.UpdateInterface();
        }
        
        public void Uninitialize() {
            foreach (ItemDatabaseObject itemDatabase in itemDatabases) {
                itemDatabase.Uninitialize();
            }
        }

        private void OnDisable() {
            Uninitialize();
        }


    }
}