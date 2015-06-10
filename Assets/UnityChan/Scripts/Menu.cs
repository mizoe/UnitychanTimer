using UnityEngine;
using System.Collections;
using System;
//using System.Timers;

namespace UnityChan
{
	public class Menu : MonoBehaviour {

		public GUIText timeText;

		private AudioClip[] voices;
		private float timer;
		private bool paused = true;
		private bool[] countDownVoice = new bool[11];

		private Animator anim;						// Animatorへの参照
		// private FaceUpdate fu;
		// private IdleChanger ic;

		//private Timer oneSecTimer = new Timer ();
		//private Timer finishTimer = new Timer ();
		private float executedTime;
		private int endTime;
		private int remainedTime; //残り時間 in Sec
		private int currentTime;

		// Use this for initialization
		void Start () {
			// 各参照の初期化
			anim = GetComponent<Animator> ();
			//fu = gameObject.GetComponent<FaceUpdate> ();
			//IdleChanger ic = gameObject.GetComponent<IdleChanger> ();

			timeText.text = "0";
			voices = new AudioClip[]{
				(AudioClip)Resources.Load("Voice/univ1136"),//0
				(AudioClip)Resources.Load("Voice/univ1147"),//1
				(AudioClip)Resources.Load("Voice/univ1148"),//2
				(AudioClip)Resources.Load("Voice/univ1149"),//3
				(AudioClip)Resources.Load("Voice/univ1150"),
				(AudioClip)Resources.Load("Voice/univ1151"),
				(AudioClip)Resources.Load("Voice/univ1152"),
				(AudioClip)Resources.Load("Voice/univ1153"),
				(AudioClip)Resources.Load("Voice/univ1154"),
				(AudioClip)Resources.Load("Voice/univ1155"),//9

				(AudioClip)Resources.Load("Voice/univ0019"),//10「こんにちは。わたし≪ユニティちゃん≫」	自己紹介（あなた編）
				(AudioClip)Resources.Load("Voice/univ1090"),//11「時間ですよーっ！」	時報
				(AudioClip)Resources.Load("Voice/univ1016"),//12「スタートっ！」	スタートとか
				(AudioClip)Resources.Load("Voice/univ0016"),//13「またね」	挨拶
				(AudioClip)Resources.Load("Voice/univ0004"),//14「ふふっ！」（笑い声）	かけ声
				(AudioClip)Resources.Load("Voice/univ0002"),//15「それっ！」（ジャンプの際の声）	かけ声
				(AudioClip)Resources.Load("Voice/univ1003"),//16「そんじゃ、始めるとしますか！」	決め台詞１
			};
			GetComponent<AudioSource>().PlayOneShot(voices[16]);
			currentTime = 0;
			//oneSecTimer.Interval = 1000;
			//oneSecTimer.Elapsed += oneSecTimerTick ();
		}

		void oneSecTimerTick(){
			timer -= 1;
		}
		
		// Update is called once per frame
		void Update () {
			if(paused == true) return;
			// timer -= Time.deltaTime;
			timer = endTime - (Time.realtimeSinceStartup-executedTime);
			//print ("timer:" + timer.ToString ());
			for(int i = 1; i<11;i++){
				if (timer <= i && timer > i-1) {
					if(countDownVoice[i]==false){
						GetComponent<AudioSource>().PlayOneShot(voices[i-1]);
						// fu.OnCallChangeFace("RANDOM");
					}
					countDownVoice[i] = true;
				};
			}

			if (timer <= 0.0f) {
				paused = true;
				timer = 0.0f;
				GetComponent<AudioSource>().PlayOneShot(voices[11]);
				anim.SetBool("Win",true);
				currentTime = 0;
				timeText.text = "0:00";
				return;
			};
			int min = 0;
			int sec = (int)timer;
			if (sec >= 60) {
				min = (int)(sec/60);
				sec = sec % 60;
			}
			string secStr;
			if (sec < 10) {
				secStr = "0" + sec.ToString ();
			} else {
				secStr = sec.ToString ();
			}
			timeText.text = min.ToString() + ":" + secStr;

		}

		void OnGUI(){
			int buttonH = 30;
			for(int i=0;i<10;i++){
				if(GUI.Button (new Rect (Screen.width/10*i, 0, Screen.width/10, buttonH), i.ToString())){
					//print (i+" pushed");
					GetComponent<AudioSource>().PlayOneShot(voices[i]);
					currentTime = currentTime*10 + i;
					timeText.text = currentTime.ToString();
					anim.SetBool("Win",false);
				}
			}
			if (GUI.Button (new Rect (0, buttonH, Screen.width / 10, buttonH), "Clear")) {
				//print ("Clear pushed");
				GetComponent<AudioSource>().PlayOneShot(voices[13]);
				paused = true;
				timeText.text = "0:00";
				currentTime = 0;
				anim.SetBool("Win",false);
				//oneSecTimer.Stop();
			}
			if (GUI.Button (new Rect (Screen.width / 10, buttonH, Screen.width / 10, buttonH), "Pause")) {
				GetComponent<AudioSource>().PlayOneShot(voices[15]);
				paused = true;
				anim.SetBool("Win",false);
				//oneSecTimer.Stop();
			}
			if (GUI.Button (new Rect (Screen.width / 10 * 7, buttonH, Screen.width / 10, buttonH), "min")) {
				//print ("min pushed");
				GetComponent<AudioSource>().PlayOneShot(voices[14]);
				timeText.text = currentTime.ToString() + ":00";
				anim.SetBool("Win",false);
				currentTime = currentTime*60;
			}
			if (GUI.Button (new Rect (Screen.width/10*8, buttonH, Screen.width/5, buttonH), "Execute")) {
				//print ("Execute pushed");
				GetComponent<AudioSource>().PlayOneShot(voices[16]);
				endTime = currentTime;
				//oneSecTimer.Start();
				executedTime = Time.realtimeSinceStartup;
				paused = false;
				anim.SetBool("Win",false);
			}
		}


	}
}