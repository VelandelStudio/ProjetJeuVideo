using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIChallengePanelBehaviour : MonoBehaviour
{

    [SerializeField] private GameObject _challengeHandler;

    [SerializeField] private Image _dungeonChallengeImage;
    [SerializeField] private Text _dungeonChallengeText;
    [SerializeField] private Image _roomChallengeImage;
    [SerializeField] private Text _roomChallengeText;

    private float timerFade = 5.0f;
    private bool _menuActivatedByPlayer;

    public void UpdateChallenges()
    {
        _challengeHandler.SetActive(true);
        timerFade = 0;
    }

    private void Update()
    {
        if (timerFade < 5.0f)
        {
            timerFade += 1.0f * Time.deltaTime;
        }
        else
        {
            if (_challengeHandler.activeSelf && !_menuActivatedByPlayer)
            {
                _challengeHandler.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _menuActivatedByPlayer = !_menuActivatedByPlayer;
            _challengeHandler.SetActive(_menuActivatedByPlayer);
        }
    }

    public void SetRoomChallenge(string text, Sprite image)
    {
        _dungeonChallengeText.text = text;
        _dungeonChallengeImage.sprite = image;
        UpdateChallenges();
    }

    public void SetDungeonChallenge(string text, Sprite image)
    {
        _roomChallengeText.text = text;
        _roomChallengeImage.sprite = image;
        UpdateChallenges();
    }
}
