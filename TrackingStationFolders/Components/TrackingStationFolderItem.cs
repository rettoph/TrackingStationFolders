using KSP.UI.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TrackingStationFolders.Components
{
    public class TrackingStationFolderItem : MonoBehaviour
    {
        private float Indentation = 15;
        private RectTransform _rectTransform;
        private Vector2 _sizeDelta;
        private Vector2 _localPosition;

        public void Awake()
        {
            _rectTransform = this.transform as RectTransform;
        }

        public void Update()
        {
            _rectTransform.sizeDelta = _sizeDelta;
            _rectTransform.localPosition = _localPosition;
        }

        public void Terminate()
        {
            this.GetComponent<TrackingStationWidget>().Terminate();
        }

        public void SetActive(bool value)
        {
            this.gameObject.SetActive(value);
        }

        public void Indent()
        {
            _sizeDelta = _rectTransform.sizeDelta - new Vector2(Indentation, 0);
            _localPosition = _rectTransform.localPosition + new Vector3(Indentation, 0, 0);
        }
    }
}
