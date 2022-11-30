using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class VolumeSliderController : MonoBehaviour
{
    [SerializeField] Slider _slider;
    Action SetUp, SliderListener;
    public VolumeType volumeType;
    public TextMeshProUGUI valueText;
    public enum VolumeType
    {
        MASTER,
        SOUNDTRACK,

    };
    private void Awake()
    {
        _slider = GetComponent<Slider>();
        switch (volumeType)
        {
            case VolumeType.MASTER:
                SetUp = SetupMasterVolumeControl;
                SliderListener = UpdateMasterVolume;
                break;
            case VolumeType.SOUNDTRACK:
                SetUp = SetupSoundTrackVolumeControl;
                SliderListener = UpdateSoundTrackVolume;
                break;
            default:
                break;
        }
    }
    private void Start()
    {
        SetUp();
        _slider.onValueChanged.AddListener(delegate { SliderListener(); });
    }
    private void OnEnable()
    {
    }
    public void UpdateMasterVolume()
    {
        AudioManager.Instance.MasterVolume = _slider.value;
        valueText.text = Math.Round(_slider.value * 100f, 1).ToString() + "%";
        // valueText.text = (slider.value * 100).ToString(".#") + "%";

    }

    public void UpdateSoundTrackVolume()
    {
        AudioManager.Instance.SountrackVolume = _slider.value;
        valueText.text = Math.Round(_slider.value * 100f, 1).ToString() + "%";
        // valueText.text = (slider.value * 100).ToString(".#") + "%";
    }

    public void SetupMasterVolumeControl()
    {
        _slider.value = AudioManager.Instance.MasterVolume;
        // _slider.onValueChanged.AddListener(delegate { UpdateMasterVolume(); });
        SliderListener = UpdateMasterVolume;
        valueText.text = Math.Round(_slider.value * 100f, 1).ToString() + "%";

    }
    public void SetupSoundTrackVolumeControl()
    {
        _slider.value = AudioManager.Instance.SountrackVolume;
        // _slider.onValueChanged.AddListener(delegate { UpdateSoundTrackVolume(); });
        SliderListener = UpdateSoundTrackVolume;
        valueText.text = Math.Round(_slider.value * 100f, 1).ToString() + "%";

    }
}
