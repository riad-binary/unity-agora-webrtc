using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using Agora.Rtc;

#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
 using UnityEngine.Android;
#endif


public class JoinChannelScript : MonoBehaviour
{

    // Fill in your app ID.
    private string _appID = "73b591f287a84022aee8565553e4fcee";
    // Fill in your channel name.
    private string _channelName = "ch1";
    // Fill in the temporary token you obtained from Agora Console.
    private string _token = "007eJxTYGhd+WP1o3KRLjED80RZx62BhVx8LG4WG7Ved+XWGS/PXqzAYG6cZGppmGZkYZ5oYWJgZJSYmmphamZqamqcapKWnJpaeKw+pSGQkUH9wAVGRgYIBPGZGZIzDBkYAGnjHJE=";
    internal IRtcEngine RtcEngine;


    #if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
        private ArrayList permissionList = new ArrayList() { Permission.Microphone };
    #endif

    // Start is called before the first frame update
    void Start()
    {
        SetupVoiceSDKEngine();
        InitEventHandler();
        SetupUI();

    }

    // Update is called once per frame
    void Update()
    {
        CheckPermissions();
    }

    private void CheckPermissions() {
        #if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
        foreach (string permission in permissionList)
        {
            if (!Permission.HasUserAuthorizedPermission(permission))
            {
                Permission.RequestUserPermission(permission);
            }
        }
        #endif
    }


    private void SetupVoiceSDKEngine()
    {
        // Create an RtcEngine instance.
        RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
        RtcEngineContext context = new RtcEngineContext(_appID, 0,
        CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING,
        AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT);
        // Initialize RtcEngine.
        RtcEngine.Initialize(context);

        Debug.Log("SetupVoiceSDKEngine");
    }



    private void InitEventHandler()
    {
        // Creates a UserEventHandler instance.
        UserEventHandler handler = new UserEventHandler(this);
        RtcEngine.InitEventHandler(handler);
    }

//
    internal class UserEventHandler : IRtcEngineEventHandler
    {
        private readonly JoinChannelScript _audioSample;

        internal UserEventHandler(JoinChannelScript audioSample)
        {
            _audioSample = audioSample;
        }

        // This callback is triggered when the local user joins the channel.
        public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
        {
        }
    }


    private void SetupUI()
    {
        GameObject go = GameObject.Find("BtnJoinChannel");
        go.GetComponent<Button>().onClick.AddListener(Join);
        go = GameObject.Find("BtnLeaveChannel");
        go.GetComponent<Button>().onClick.AddListener(Leave);
    }


    public void Join()
    {
        // Enables the audio module.
        RtcEngine.EnableAudio();
        // Sets the user role ad broadcaster.
        RtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        // Joins a channel.
        RtcEngine.JoinChannel(_token, _channelName);
        Debug.Log("Join");
    }


    public void Leave()
    {
        // Leaves the channel.
        RtcEngine.LeaveChannel();
        // Disable the audio modules.
        RtcEngine.DisableAudio();
        Debug.Log("Leave");
    }


    void OnApplicationQuit()
    {
        if (RtcEngine != null)
        {
            Leave();
            RtcEngine.Dispose();
            RtcEngine = null;

        }
    }






}
