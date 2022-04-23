using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Serialization;

namespace Normal.Realtime
{
    [RequireComponent(typeof(Realtime))]
    public class RealtimeAvatarManagerFork : MonoBehaviour
    {
#pragma warning disable 0649 // Disable variable is never assigned to warning.
        [FormerlySerializedAs("_avatarPrefab")]
        [SerializeField] private GameObject _localAvatarPrefab;
        [SerializeField] private RealtimeAvatarFork.LocalPlayer _localPlayer;
        //[SerializeField] private GameObject[] _avatarPrefabArray;
#pragma warning restore 0649

        public GameObject localAvatarPrefab { get { return _localAvatarPrefab; } set { SetLocalAvatarPrefab(value); } }

        public RealtimeAvatarFork localAvatar { get; private set; }
        public Dictionary<int, RealtimeAvatarFork> avatars { get; private set; }

        public delegate void AvatarCreatedDestroyed(RealtimeAvatarManagerFork avatarManager, RealtimeAvatarFork avatar, bool isLocalAvatar);
        public event AvatarCreatedDestroyed avatarCreated;
        public event AvatarCreatedDestroyed avatarDestroyed;

        private Realtime _realtime;

        void Awake()
        {
            _realtime = GetComponent<Realtime>();
            _realtime.didConnectToRoom += DidConnectToRoom;

            if (_localPlayer == null)
                _localPlayer = new RealtimeAvatarFork.LocalPlayer();

            avatars = new Dictionary<int, RealtimeAvatarFork>();
        }

        private void OnEnable()
        {
            // Create avatar if we're already connected
            if (_realtime.connected)
                CreateAvatarIfNeeded();
        }

        private void OnDisable()
        {

            // Destroy avatar if needed
            DestroyAvatarIfNeeded();
        }

        void OnDestroy()
        {
            _realtime.didConnectToRoom -= DidConnectToRoom;
        }

        void DidConnectToRoom(Realtime room)
        {
            if (!gameObject.activeInHierarchy || !enabled)
                return;
            /* switch (_realtime.clientID)
             {
                 case 0:
                     _localAvatarPrefab = _avatarPrefabArray[0];
                     break;
                 case 1:
                     _localAvatarPrefab = _avatarPrefabArray[1];
                     break;
             }*/
            // Create avatar
            CreateAvatarIfNeeded();
        }

        public static RealtimeAvatarFork.DeviceType GetRealtimeAvatarDeviceTypeForLocalPlayer()
        {
            switch (XRSettings.loadedDeviceName)
            {
                case "OpenVR":
                    return RealtimeAvatarFork.DeviceType.OpenVR;
                case "Oculus":
                    return RealtimeAvatarFork.DeviceType.Oculus;
                default:
                    return RealtimeAvatarFork.DeviceType.Unknown;
            }
        }

        public void _RegisterAvatar(int clientID, RealtimeAvatarFork avatar)
        {
            if (avatars.ContainsKey(clientID))
            {
                Debug.LogError("RealtimeAvatar registered more than once for the same clientID (" + clientID + "). This is a bug!");
            }
            avatars[clientID] = avatar;

            // Fire event
            if (avatarCreated != null)
            {
                try
                {
                    avatarCreated(this, avatar, clientID == _realtime.clientID);
                }
                catch (System.Exception exception)
                {
                    Debug.LogException(exception);
                }
            }
        }

        public void _UnregisterAvatar(RealtimeAvatarFork avatar)
        {
            bool isLocalAvatar = false;

            List<KeyValuePair<int, RealtimeAvatarFork>> matchingAvatars = avatars.Where(keyValuePair => keyValuePair.Value == avatar).ToList();
            foreach (KeyValuePair<int, RealtimeAvatarFork> matchingAvatar in matchingAvatars)
            {
                int avatarClientID = matchingAvatar.Key;
                avatars.Remove(avatarClientID);

                isLocalAvatar = isLocalAvatar || avatarClientID == _realtime.clientID;
            }

            // Fire event
            if (avatarDestroyed != null)
            {
                try
                {
                    avatarDestroyed(this, avatar, isLocalAvatar);
                }
                catch (System.Exception exception)
                {
                    Debug.LogException(exception);
                }
            }
        }

        private void SetLocalAvatarPrefab(GameObject localAvatarPrefab)
        {
            if (localAvatarPrefab == _localAvatarPrefab)
                return;

            _localAvatarPrefab = localAvatarPrefab;

            // Replace the existing avatar if we've already instantiated the old prefab.
            if (localAvatar != null)
            {
                DestroyAvatarIfNeeded();
                avatars.Remove(_realtime.clientID);
                CreateAvatarIfNeeded();
            }
        }

        public void CreateAvatarIfNeeded()
        {
            if (!_realtime.connected)
            {
                Debug.LogError("RealtimeAvatarManager: Unable to create avatar. Realtime is not connected to a room.");
                return;
            }

            if (localAvatar != null)
                return;

            if (_localAvatarPrefab == null)
            {
                Debug.LogWarning("Realtime Avatars local avatar prefab is null. No avatar prefab will be instantiated for the local player.");
                return;
            }

            GameObject avatarGameObject = Realtime.Instantiate(_localAvatarPrefab.name, new Realtime.InstantiateOptions
            {
                ownedByClient = true,
                preventOwnershipTakeover = true,
                destroyWhenOwnerLeaves = true,
                destroyWhenLastClientLeaves = true,
                useInstance = _realtime,
            });

            if (avatarGameObject == null)
            {
                Debug.LogError("RealtimeAvatarManager: Failed to instantiate RealtimeAvatar prefab for the local player.");
                return;
            }

            localAvatar = avatarGameObject.GetComponent<RealtimeAvatarFork>();
            if (localAvatar == null)
            {
                Debug.LogError("RealtimeAvatarManager: Successfully instantiated avatar prefab, but could not find the RealtimeAvatar component.");
                return;
            }

            localAvatar.localPlayer = _localPlayer;
            localAvatar.deviceType = GetRealtimeAvatarDeviceTypeForLocalPlayer();
#if !UNITY_2020_2_OR_NEWER
#pragma warning disable 0618
            // Unity deprecated this API in 2020.2 without a clear replacement.
            localAvatar.deviceModel = XRDevice.model;
#pragma warning restore 0618
#endif
        }

        public void DestroyAvatarIfNeeded()
        {
            if (localAvatar == null)
                return;

            Realtime.Destroy(localAvatar.gameObject);

            localAvatar = null;
        }
    }
}