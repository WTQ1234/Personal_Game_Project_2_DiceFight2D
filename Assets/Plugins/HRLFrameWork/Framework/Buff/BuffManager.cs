using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRL
{
    public class BuffManager : MonoSingleton<BuffManager>
    {
        private Dictionary<int, BuffInfo> dict_all_info;

        protected override void Awake()
        {
            base.Awake();
            dict_all_info = ConfigManager.Instance.GetAllInfo<BuffInfo>();
        }

        public BuffBase AddBuffByCfgId(GameObject _gameObject, int _cfg_id)
        {
            BuffController controller = _GetBuffController(_gameObject);
            BuffInfo buffInfo = _GetBuffInfo(_cfg_id);
            BuffBase buff = controller.AddBuffByCfgId(buffInfo);
            return buff;
        }

        private BuffController _GetBuffController(GameObject _gameObject)
        {
            BuffController controller = _gameObject.GetComponent<BuffController>();
            if (controller == null)
            {
                controller = _gameObject.AddComponent<BuffController>();
            }
            return controller;
        }

        private BuffInfo _GetBuffInfo(int _cfg_id)
        {
            return dict_all_info[_cfg_id];
        }
    }
}
