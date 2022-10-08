using DG.Tweening;
using JK.Injection;
using JK.Injection.PropertyDrawers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LudumDare51.UI
{
    [DisallowMultipleComponent]
    public class TalkPanel : MonoBehaviour
    {
        #region Inspector

        public CanvasGroup group;

        [Injected]
        public NextScene nextScene;

        private void Reset()
        {
            group = GetComponentInChildren<CanvasGroup>();
        }

        #endregion

        private void Awake()
        {
            Context context = Context.Find(this);
            nextScene = context.Get<NextScene>(this);

            // avoid OnEnable
            if (Application.isPlaying && transform.GetSiblingIndex() != 0)
                gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            group.alpha = 0;
            group.DOFade(1, 0.25f);
        }

        private static List<Text> buffer = new List<Text>(4);

        private void Start()
        {
            GetComponentsInChildren(buffer);

            foreach (var text in buffer)
                text.text = Tokens.Format(text.text, isMultiplayer: false);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                enabled = false;
                group.DOFade(0, 0.25f).onComplete += () =>
                {
                    int index = transform.GetSiblingIndex();

                    if (index < transform.parent.childCount - 1)
                    {
                        gameObject.SetActive(false);
                        transform.parent.GetChild(index + 1).gameObject.SetActive(true);
                    }
                    else
                    {
                        SceneManager.LoadSceneAsync(nextScene.sceneName);
                    }
                };
            }
        }
    }
}