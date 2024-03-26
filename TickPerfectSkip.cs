// Decompiled with JetBrains decompiler
// Type: TickPerfectSkip
// Assembly: TickPerfectSkip, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3BF8275E-6C12-4DD5-8A51-43187FEE1A09
// Assembly location: D:\Steam\steamapps\workshop\content\718670\2913462842\dll\TickPerfectSkip.dll

using SecretHistories.Infrastructure;
using SecretHistories.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TickPerfectSkip : MonoBehaviour
{
  public static ValueTracker<int> skipSpeed;
  public static KeybindTracker keySkip;
  private int _skipTicks = 0;

  public void Start() => SceneManager.sceneLoaded += Load;

  public void OnDestroy() => SceneManager.sceneLoaded -= Load;

  public async void Update()
  {
    if (Watchman.Get<LocalNexus>() == null || Watchman.Get<LocalNexus>().PlayerInputDisabled())
        return;
    if (TickPerfectSkip.keySkip.wasPressedThisFrame())
    {
        this._skipTicks += (int)((double)(TheWheel.GetNextCardTime() * 100) + 0.01);
        NoonUtility.Log(string.Format("TickPerfectSkip: Ticks {0}", this._skipTicks) );
    }

    await Settler.AwaitSettled();
    if (this._skipTicks > 0) {
        while (this._skipTicks > 0) {
            bool _ = TrySkip(400)
                || TrySkip(100)
                || TrySkip(25)
                || TrySkip(1);
            if (TickPerfectSkip.skipSpeed.current > 0f) {break;}
            await Settler.AwaitSettled();
        }
    }
  }

    public bool TrySkip(int ticks)
    {
        if (ticks == 1) {
            Watchman.Get<Heart>().Beat(0.015625f, 0.0f);
        } else if (this._skipTicks - TickPerfectSkip.skipSpeed.current < ticks) {
            return false;
        } else {
            NoonUtility.Log(string.Format("TickPerfectSkip: Skip {0}", 0.01 * ticks) );
            Watchman.Get<Heart>().Beat((float)(0.01 * ticks), 0.5f);
        }
        this._skipTicks -= ticks;
        return true;
    }

  public static void Initialise()
  {
    new GameObject().AddComponent<TickPerfectSkip>();
    NoonUtility.Log("TickPerfectSkip: Initialised");
  }

  private void Load(Scene scene, LoadSceneMode mode)
  {
    if (!(scene.name == "S3Menu"))
      return;
    try
    {
        TickPerfectSkip.skipSpeed = new ValueTracker<int>("SkipSpeed", new int[3] { 0, 10, 70 });
        TickPerfectSkip.keySkip = new KeybindTracker("KeyTickPerfectSkip");
    }
    catch (Exception ex)
    {
      NoonUtility.LogException(ex);
    }
    NoonUtility.Log("TickPerfectSkip: Trackers Started");
  }

}
