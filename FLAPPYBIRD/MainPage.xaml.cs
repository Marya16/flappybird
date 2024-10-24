

namespace FLAPPYBIRD;

public partial class MainPage : ContentPage
{
	
const int gravidade = 8;
const int tempoEntreFrames = 25;
bool estaMorto = true;
double larguraJanela = 0;
double alturaJanela = 0;
int velocidade = 10;
const int maxTempoPulando = 3;
int tempoPulando = 0;
bool estaPulando = false;
const int forcaPulo = 40;
const int aberturaMin = 200;
int score = 0;
	public MainPage()
	{
		InitializeComponent();
		
	}
	void AplicaGravidade()
	{
		guido.TranslationY += gravidade;
	}
	void AplicaPulo()
	{
		guido.TranslationY -=forcaPulo;
		tempoPulando++;
		if(tempoPulando >= maxTempoPulando)
		{
			estaPulando=false;
			tempoPulando=0;
		}
	}
	async Task Desenhar ()
	{
      while (! estaMorto)
	  {
         if (estaPulando)
		   AplicaPulo();
		 else
		 AplicaGravidade(); 
         GerenciaCanos();
		 if (VerificaColisao())
		 {
			estaMorto=true;
			labelfrase.Text = "VOCE PASSOU POR:" + score + "Canos";
			frameGameOver.IsVisible = true;
			break;
		 }
		 await Task.Delay(tempoEntreFrames);
	  }
	}
    protected override void OnSizeAllocated(double w, double h)
    {
    base.OnSizeAllocated(w, h);
	larguraJanela=w;
	alturaJanela=h;
    }

	void GerenciaCanos()
	{
		imgCanocima.TranslationX-= velocidade;
		imgCanobaixo.TranslationX-= velocidade;
		if(imgCanobaixo.TranslationX<=-larguraJanela)
		{
			imgCanobaixo.TranslationX=0;
			imgCanocima.TranslationX=0;
			var alturaMax=-100;
			var alturaMin=-imgCanobaixo.HeightRequest;
			imgCanocima.TranslationY = Random.Shared.Next((int)alturaMin, (int)alturaMax);
			imgCanobaixo.TranslationY = imgCanocima.TranslationY + alturaMin + aberturaMin + imgCanobaixo.HeightRequest;
			score++;
			if(score %2 == 0)
            velocidade++;
			labelScore.Text="Canos:" + score.ToString("D3");
		}
	}

	void OnGameOverClicked(object s,TappedEventArgs a)
	{
		frameGameOver.IsVisible=false;
		Inicializar();
		Desenhar();
	}

	void Inicializar()
	{
		estaMorto=false;
		guido.TranslationY=0;
        imgCanocima.TranslationX=-larguraJanela;
		imgCanobaixo.TranslationX=-larguraJanela;
		guido.TranslationX=0;
		guido.TranslationY=0;
		score=0;
		GerenciaCanos();
	}
   bool VerificaColisaoTeto()
	{
		var minY=-alturaJanela/2;
		if (guido.TranslationY<=minY)
		   return true;
		else
		   return false;
	}
   bool VerificaColisaoChao()
   {
	var maxY = alturaJanela/2 - chao.HeightRequest;
	if (guido.TranslationY>=maxY)
       return true;
	else
	   return false;  
   }
   bool VerificaColisao()
   {
	return VerificaColisaoTeto()||
	       VerificaColisaoChao()||
		   VerificaColisaoCanocima()||
		   VerificaColosaoCanobaixo();       
   }
   bool VerificaColisaoCanocima()
   {
	var posHGuido = (larguraJanela/2)-(guido.WidthRequest/2);
	var posVGuido = (alturaJanela/2)-(guido.HeightRequest/2);
    if (posHGuido >= Math.Abs(imgCanocima.TranslationX)-imgCanocima.WidthRequest &&
	    posHGuido <= Math.Abs(imgCanocima.TranslationX)+imgCanocima.WidthRequest &&
		posVGuido <= imgCanocima.HeightRequest + imgCanocima.TranslationY)
		
			return true;
		else
			return false;
	
   }
   bool VerificaColosaoCanobaixo()
   {
	var posHGuido = (larguraJanela/2) - (guido.WidthRequest/2);
	var posVGuido = (alturaJanela/2) + (guido.HeightRequest/2) + guido.TranslationY;
	var yMaxCano = imgCanocima.HeightRequest + imgCanocima.TranslationY + aberturaMin;
	if (posHGuido >= Math.Abs(imgCanobaixo.TranslationX) - imgCanobaixo.WidthRequest &&
	    posHGuido <= Math.Abs(imgCanobaixo.TranslationX) + imgCanobaixo.WidthRequest &&
		posVGuido >= yMaxCano)
		
            return true;
		else
			return false;
		
   }
   void GuidoSobe(object sender, EventArgs e)
   {
		estaPulando=true;
   }

}

