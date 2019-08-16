using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class CharacterController : MonoBehaviour
    {
        GameObject background;
        SceneLooper sceneLooper;
        
        GameObject tree;
        BoxCollider2D treeBc;

        public float loudness = 0;
        public float vulume = 1f;
        public static float sensitivity = 80;
        public AudioClip _fallSound;
        public AudioClip _hitSound;
        public AudioClip _scoreSound;

        public static bool go = false;
        Rigidbody2D _rigidbody2D;
        AudioSource _mic;
        PolygonCollider2D _collider2D;
        Animator _anim;

        Quaternion _fromRotation;
        Quaternion _toRotation;
        float rotationSmooth = 3f;
        float upDegree = 20f;
        float downDegree = 20f;
        int score = 0;
        int bestScore = 0;
        public static int gameOverCount = 0;
        int removeAds = 0;
        public static float tutorialTiming = 0;

        IEnumerator Start()
        {
            background = GameObject.FindGameObjectWithTag("Background");
            sceneLooper = background.GetComponent<SceneLooper>();
            _anim = GetComponent<Animator>();
            _mic = GetComponent<AudioSource>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<PolygonCollider2D>();

            tree = background.transform.GetChild(2).gameObject;
            treeBc = tree.GetComponent<BoxCollider2D>();

            //Voice Control
            yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
            if (Application.HasUserAuthorization(UserAuthorization.Microphone))
            {
                _mic.clip = Microphone.Start(null, true, 10, 44100);
                _mic.loop = true;
                _mic.mute = true;
                while (!(Microphone.GetPosition(null) > 0)) { }
                _mic.Play();
            }
            else
            {
                //Error
            }

            bestScore = PlayerPrefs.GetInt("BestScore");

            Debug.Log("game over count: " + gameOverCount);
            removeAds = PlayerPrefs.GetInt("RemoveAds");
            if (gameOverCount > 2 && removeAds != 1)
            {
                GameObject.FindGameObjectWithTag("InterAds").GetComponent<InterAdsController>().ShowAds();
                gameOverCount = 0;
            }
        }

        void Update()
        {
            VoiceControl();

            tutorialTiming += Time.deltaTime;

            if (!go && loudness > 1.3f)
            {
                go = true;
                treeBc.isTrigger = true;
            }
            else if(MenuController.currentMenu == MenuController.MenuStates.GetReadyMenu && tutorialTiming > 5f)
            {
                MenuController.currentMenu = MenuController.MenuStates.TutorialMenu;
                tutorialTiming = 0;
            }

            if (go)
            {
                sceneLooper.enabled = true;
                _fromRotation = transform.rotation;
                _toRotation = Quaternion.Euler(0, 0, _rigidbody2D.velocity.y > 0 ? upDegree : - downDegree);
                transform.rotation = Quaternion.Lerp(_fromRotation, _toRotation, rotationSmooth * Time.deltaTime);
            }

            
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.tag == "Finish"){
                _mic.mute = true;
                AudioSource.PlayClipAtPoint(_hitSound, transform.position, vulume);
                _collider2D.enabled = false;
                StartCoroutine(PlayFallSound(0.5f));
                _anim.SetBool("dead", true);
                _rigidbody2D.gravityScale = 0.4f;
                StartCoroutine(StopCamera(2f));
            }
            
            if (other.gameObject.tag == "Tree")
            {
                _anim.SetBool("idle", true);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.tag == "Tree")
            {
                _anim.SetBool("idle", false);
            }
            if (other.gameObject.tag == "Point")
            {
                //AudioSource.PlayClipAtPoint(_scoreSound, transform.position, vulume);
                score++;

                if (score > bestScore)
                {
                    bestScore = score;
                    PlayerPrefs.SetInt("BestScore", bestScore);
                    MenuController.newBestScore = true;
                }
                MenuController.score = score;
            }
        }

        IEnumerator StopCamera(float time)
        {
            yield return new WaitForSeconds(time);

            sceneLooper.StopScene();
            Invoke("GameOver", 3f);
        }

        IEnumerator PlayFallSound(float time)
        {
            yield return new WaitForSeconds(time);
            AudioSource.PlayClipAtPoint(_fallSound, transform.position, vulume);
        }

        void VoiceControl()
        {
            loudness = GetAveragedVolume() * sensitivity;

            if (loudness > 1 && loudness < 1.7)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 1);
            }
            else if (loudness > 1.7 && loudness < 3)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 2);
            }
            else if (loudness > 3 && loudness < 5)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 3);
            }
            else if (loudness > 5 && loudness < 10)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 4);
            }
            else if (loudness > 10)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 5);
            }
        }

        //Get Averaged Volume
        float GetAveragedVolume()
        {
            float[] data = new float[256];
            float a = 0;
            _mic.GetOutputData(data, 0);

            foreach (float s in data)
            {
                a += Mathf.Abs(s);
            }
            return a / 256;
        }

        void GameOver()
        {
            gameOverCount++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            go = false;
        }
    }
}
