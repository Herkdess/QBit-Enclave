using Sirenix.OdinInspector.Editor;
using UnityEngine;
namespace RPGSystems.Editor {
    public class RPGEditWindow : OdinMenuEditorWindow {

        protected override OdinMenuTree BuildMenuTree() {
            OdinMenuTree tree = new OdinMenuTree(true);
            return tree;
        }
    }
    
}