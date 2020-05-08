using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Slider healthBar;
    public Slider dashCooldown;
    public Slider hookCooldown;

    public void SetHealthBarMaxValue(float value)
    {
        healthBar.maxValue = value;
        healthBar.value = value;
    }

    public void SetHealthBarValue(float value)
    {
        healthBar.value = value;
    }

    public void SetDashCooldownMaxValue(float value)
    {
        dashCooldown.maxValue = value;
        dashCooldown.value = value;
    }

    public void SetDashCooldownValue(float value)
    {
        dashCooldown.value = value;
    }

    public void SetHookCooldownMaxValue(float value)
    {
        hookCooldown.maxValue = value;
        hookCooldown.value = value;
    }

    public void SetHookCooldownValue(float value)
    {
        hookCooldown.value = value;
    }
}
