using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wboard{
	public class Prog{
		static int[,] board = new int[10,10];
		static int[] head = new int[]{4,4};
		static List<int[]> snake = new List<int[]>();
		static int[] apple = new int[]{8,5};
		
		
		static string[] chares = new string[3];
		static bool game = true;
		static bool exit = false;
		static string napis = "haha. Przegra³eœ!!";
		static string clean = "                       ";
		static int dx = 1;
		static int dy = 0;
		static Random rnd = new Random();
		
		static int win = 10;
		static int dificulty = 250;
		
		
		
		static void Main(string[] args){
			Console.CursorVisible = false;
			chares = File.ReadAllLines("baner.txt");
			info inf = JsonSerializer.Deserialize<info>(File.ReadAllText("config.json"));
			board = new int[inf.board_x,inf.board_y];
			win = inf.win;
			dificulty = inf.dificulty;
			
			gen_food();
			
			while(!exit){
				Console.SetCursorPosition(0, 2);
				for(int i=0;i<13;i++)Console.WriteLine(clean);
				Console.SetCursorPosition(0, 2);
				Console.Write("Naciœnik: Enter, aby zagraæ;    x, aby wyjœæ;");
				Console.ReadLine();
				while(game && !exit){
					loop();
					Thread.Sleep(dificulty);
				}
				head[0] = 4;
				head[1] = 4;
				snake = new List<int[]>();
				gen_food();
				Console.WriteLine(napis+"\nNaciœnik Enter, aby zagraæ ponownie");
				Console.ReadLine();
				game = true;
			}
			
			//...
		}
		static void wys(){
			Console.SetCursorPosition(0, 3);
			Console.WriteLine(DateTime.Now+"  d³ugoœæ:"+snake.Count);
			for(int i=0;i<board.GetLength(0);i++){
				for(int j=0;j<board.GetLength(1);j++){
					switch(board[i,j]){
						case 0:
							Console.Write(chares[0]);
						break;
						case 1:
							Console.Write(chares[1]);
						break;
						case 2:
							Console.Write(chares[2]);
						break;
					}
				}
				Console.WriteLine();
			}
		}
		static void loop(){
			for(int i=0;i<board.GetLength(0);i++){
				for(int j=0;j<board.GetLength(1);j++){
					board[i, j] = 0;
				}
			}
			keyy();
			head[0] += dx;
			head[1] += dy;
			snake.Insert(0, new int[]{head[0], head[1]});
			board[apple[1], apple[0]] = 2;
			
			try{
				for(int i=0;i<snake.Count;i++){//add snake to board
					board[snake[i][1], snake[i][0]] = 1;
				}
			}catch{
				def();
			}
			if(snake.Count > win){
				won();
			}
			
			for(int i=1; i<snake.Count; i++){    
				if(snake[i][0] == snake[0][0] && snake[i][1] == snake[0][1]){
					def();
				}
			}
			if(head[0] == apple[0] && head[1] == apple[1]){//eat
				gen_food();
				wys();
			}else{
				wys();
				snake.RemoveAt(snake.Count-1);
			}
		}
		
		static void def(){
			napis = "Haha. Przegra³eœ!!";
			game = false;
		}
		static void won(){
			napis = "Brawo. Wygra³eœ";
			game = false;
		}
		static void keyy(){
			if(Console.KeyAvailable){
				var temp = Console.ReadKey().Key;
				if(temp == ConsoleKey.W){
					dy = -1;
					dx = 0;
				}else
				if(temp == ConsoleKey.S){
					dy = 1;
					dx = 0;
				}else
				if(temp == ConsoleKey.A){
					dy = 0;
					dx = -1;
				}else
				if(temp == ConsoleKey.D){
					dy = 0;
					dx = 1;
				}else
				if(temp == ConsoleKey.X){
					exit = true;
				}
			}
		}
		static void gen_food(){
			apple[0] = rnd.Next(0, board.GetLength(0)-1);
			apple[1] = rnd.Next(0, board.GetLength(1)-1);
			for(int i=0; i< snake.Count; i++){
				if(snake[i][0] == apple[0] && snake[i][1] == apple[1]){
					gen_food();
				}
			}
		}
		
	}//class
	public class info{
		public int board_x{get;set;}
		public int board_y{get;set;}
		public int win{get;set;}
		public int dificulty{get;set;}
	}
}