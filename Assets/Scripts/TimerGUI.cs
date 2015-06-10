using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace UnityChan
{
	public class TimerGUI : MonoBehaviour {
		private AudioClip[] voices;
		private AudioSource audioSrc;
		private float timer;
		private bool[] countDownVoice = new bool[11];
		private Animator anim; // Animatorへの参照

		private float executedTime;
		private int endTime;
		private int remainedTime; //残り時間 in Sec
		private int currentTime;

		private DateTime targetDT;
		private TimeSpan targetTS;
		private DateTime startDT;
		private DateTime nowDT;
		private TimeSpan diffTS;

		public Canvas canvas;
		private Text timerText;
		private Text clockText;
		private bool paused;

		private string INIT_STRING = "INPUT TIME";
		private FaceUpdate fu;
		private IdleChanger ic;

		// Use this for initialization
		void Start () {
			nowDT = DateTime.Now;
			targetTS = new TimeSpan (0, 0, 0);
			fu = gameObject.GetComponent<FaceUpdate> ();
			ic = gameObject.GetComponent<IdleChanger> ();
			audioSrc = GetComponent<AudioSource> ();
			//backgroundでも動く
			Application.runInBackground = true;
			paused = true;
			anim = GetComponent<Animator> ();
			foreach (Transform child in canvas.transform){
				if(child.name == "RestTime"){
					timerText = child.gameObject.GetComponent<Text>();
					timerText.text = INIT_STRING;
				}else if(child.name == "Clock"){
					clockText = child.gameObject.GetComponent<Text>();
					printClockText();
				}
			}
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
			audioSrc.PlayOneShot(voices[16]);
			currentTime = 0;
			//Debug.Log (audioSrc.ToString());
			//Debug.Log (voices[0]);
			//GetComponent<AudioSource> ().PlayOneShot((AudioClip)Resources.Load("univ0001"));
		}
		
		// Update is called once per frame
		void Update () {
			printClockText ();

			if(paused == true) return;
			nowDT = DateTime.Now;
			diffTS = targetDT.Subtract(nowDT);
			Debug.Log (diffTS.TotalSeconds);
			return;
			if (diffTS.TotalSeconds <= 0.0f) {
				paused = true;
				audioSrc.PlayOneShot(voices[11]);
				anim.SetBool("Win",true);
				timerText.text = "TIME UP";
				return;
			};

			if (diffTS.TotalSeconds <= 10) {
				for (int i = 1; i<=10; i++) {
					if (diffTS.TotalSeconds <= i && diffTS.TotalSeconds > i - 1) {
						if (countDownVoice [i] == false) {
							audioSrc.PlayOneShot (voices [i - 1]);
						}
						countDownVoice [i] = true;
					}
				}
			}
			printTimerText (diffTS);
			fu.OnCallChangeFace("RANDOM");
		}

		private void printTimerText(TimeSpan ts){
			timerText.text = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
		}

		private void printClockText(){
			clockText.text = string.Format(DateTime.Now.ToString("HH:mm:ss"));
		}

		public void PushNumber(int i){
			audioSrc.PlayOneShot(voices[i]);
			targetTS = new TimeSpan (targetTS.Hours, targetTS.Minutes, targetTS.Seconds*10 + i);
			Debug.Log (targetTS.ToString ());
			printTimerText (targetTS);
			anim.SetBool("Win",false);
		}

		public void PushMin(){
			audioSrc.PlayOneShot(voices[14]);
			targetTS = new TimeSpan(targetTS.Minutes,targetTS.Seconds,0);
			printTimerText (targetTS);
			anim.SetBool("Win",false);
		}

		public void PushClear(){
			audioSrc.PlayOneShot (voices [13]);
			paused = true;
			timerText.text = INIT_STRING;
			targetTS = new TimeSpan(0);
			anim.SetBool ("Win", false);
		}

		public void PushOK(){
			audioSrc.PlayOneShot(voices[16]);
			targetDT = nowDT.Add (targetTS);
			paused = false;
			anim.SetBool("Win",false);
		}
	}
}