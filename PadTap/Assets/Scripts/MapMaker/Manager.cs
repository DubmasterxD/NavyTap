using PadTap.Core;
using System.Collections.Generic;
using UnityEngine;

namespace PadTap.MapMaker
{
    public class Manager : MonoBehaviour
    {
        private AudioSource audioSource = null;
        private Map map = null;
        private Map.Point currentPoint = null;
        private float selectionStartTime = 0;
        private float selectionEndTime = 0;
        private List<Map.Point> selection = null;
        private float currentTime = 0;
        private float playbackSpeed = 1;
        private float deltaTime = 1;
        private bool givenCopyright = false;

        private int minRows = 1;
        private int maxRows = 6;
        private int minColumns = 1;
        private int maxColumns = 6;
        private float minIndicatorLifespan = 0.1f;
        private float maxIndicatorLifespan = 10;

        private void Update()
        {
            
        }
    }
}