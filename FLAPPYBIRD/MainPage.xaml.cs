using System.Reflection.Metadata.Ecma335;

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
		 if (VerificaColisaoChao())
		 {
			estaMorto=true;
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
	var maxY = alturaJanela/2;
	if (guido.TranslationY>=maxY)
       return true;
	else
	   return false;  
   }
   bool VerificaColisao()
   {
	if (!estaMorto)
	{
		if (VerificaColisaoTeto()||
		    VerificaColisaoChao())
			{
				return true;
			}
	}
	            return false;
   }

   void GuidoSobe(object sender, EventArgs e)
   {
		estaPulando=true;
   }
}
