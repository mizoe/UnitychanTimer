using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

		public Canvas canvas;
		private Text timerText;
		private bool paused;

		private string INIT_STRING = "INPUT TIME";
		private FaceUpdate fu;
		private IdleChanger ic;

		// Use this for initialization
		void Start () {
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
					break;
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
			//GetComponent<AudioSource>().PlayOneShot(voices[10]);
			currentTime = 0;
		}
		
		// Update is called once per frame
		void Update () {
			if(paused == true) return;

			timer = endTime - (Time.realtimeSinceStartup-executedTime);
			//print ("timer:" + timer.ToString ());
			if (timer <= 0.0f) {
				paused = true;
				timer = 0.0f;
				audioSrc.PlayOneShot(voices[11]);
				anim.SetBool("Win",true);
				currentTime = 0;
				timerText.text = "TIME UP";
				return;
			};

			if (timer < 11) {
				for (int i = 1; i<11; i++) {
					if (timer <= i && timer > i - 1) {
						if (countDownVoice [i] == false) {
							audioSrc.PlayOneShot (voices [i - 1]);
						}
						countDownVoice [i] = true;
					}
					;
				}
			}
			
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
			timerText.text = min.ToString() + ":" + secStr;
			fu.OnCallChangeFace("RANDOM");
			if (sec % 5 == 0) {

			}
		}

		public void PushNumber(int i){
			audioSrc.PlayOneShot(voices[i]);
			currentTime = currentTime*10 + i;
			timerText.text = currentTime.ToString();
			anim.SetBool("Win",false);
		}

		public void PushMin(){
			audioSrc.PlayOneShot(voices[14]);
			timerText.text = currentTime.ToString() + ":00";
			anim.SetBool("Win",false);
			currentTime = currentTime*60;
		}

		public void PushClear(){
			audioSrc.PlayOneShot (voices [13]);
			paused = true;
			timerText.text = INIT_STRING;
			currentTime = 0;
			anim.SetBool ("Win", false);
		}

		public void PushOK(){
			audioSrc.PlayOneShot(voices[16]);
			endTime = currentTime;
			executedTime = Time.realtimeSinceStartup;
			paused = false;
			anim.SetBool("Win",false);
		}
	}
}