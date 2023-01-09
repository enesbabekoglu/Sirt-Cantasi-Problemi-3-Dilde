using System;
using System.Collections.Generic;
using System.IO;

namespace SirtCantasiProblemi{
    
    class Program{
        
        static (int kapasite, List<(int agirlik, int fiyat)> urunler) UrunleriOku(string filename){

            // Okunan dosyadaki öğeleri okur ve kapasiteyi ve öğeleri bir çift olarak döndürür
            var urunler = new List<(int agirlik, int fiyat)>();

            using (var okuyucu = new StreamReader(filename)){

                var kapasite = int.Parse(okuyucu.ReadLine().Trim());

                string satir;

                while ((satir = okuyucu.ReadLine()) != null){

                    var parcala = satir.Trim().Split();
                    var agirlik = int.Parse(parcala[0]);
                    var fiyat = int.Parse(parcala[1]);
                    urunler.Add((agirlik, fiyat));

                }

                return (kapasite, urunler);

            }
        }

        static List<(int agirlik, int fiyat)> UrunleriSec(int kapasite, List<(int agirlik, int fiyat)> urunler){

            // Burada maksimum fiyatı elde etmek için gereken en iyi ürün listesini seçer.

            var urunSayisi = urunler.Count;
            var dp = new int[urunSayisi + 1][];

            for (int i = 0; i < urunSayisi + 1; i++){
                dp[i] = new int[kapasite + 1];
            }

            for (int i = 0; i < urunSayisi + 1; i++){

                for (int j = 0; j < kapasite + 1; j++){

                    if (i == 0 || j == 0){
                        dp[i][j] = 0;
                    }else if (urunler[i - 1].agirlik <= j){
                        dp[i][j] = Math.Max(dp[i - 1][j], dp[i - 1][j - urunler[i - 1].agirlik] + urunler[i - 1].fiyat);
                    }else{
                        dp[i][j] = dp[i - 1][j];
                    }

                }
            }

            var secilenUrunler = new List<(int agirlik, int fiyat)>();

            for (int i = urunSayisi, j = kapasite; i > 0 && j > 0; i--){

                if (dp[i][j] != dp[i - 1][j]){
                    secilenUrunler.Add(urunler[i - 1]);
                    j -= urunler[i - 1].agirlik;
                }

            }

            return secilenUrunler;

        }

        static void UrunleriYazdir(int kapasite, List<(int agirlik, int fiyat)> urunler){

            // Verilen kapasite ve öğeleri konsola yazdırır
            Console.WriteLine("Canta Kapasitesi: " + kapasite + " KG");
            Console.WriteLine("\nURUNLER");

            for (int i = 0; i < urunler.Count; i++){
                var (agirlik, fiyat) = urunler[i];
                Console.WriteLine((i + 1) + ". Urun - Agirlik: " + agirlik + " KG / Fiyat: " + fiyat + " TL");
            }

            var secilenUrunler = UrunleriSec(kapasite, urunler);

            Console.WriteLine("\nCANTADAKI URUNLER");
            foreach (var urun in secilenUrunler)
            {
                Console.WriteLine("Agirlik: " + urun.agirlik + " KG / Fiyat: " + urun.fiyat + " TL");
            }

        }

        static int Canta(int kapasite, List<(int agirlik, int fiyat)> urunler){

            // Verilen listeden öğeler seçilerek elde edilebilecek en yüksek değeri döndürür.

            var urunSayisi = urunler.Count;
            var dp = new int[urunSayisi + 1][];

            for (int i = 0; i < urunSayisi + 1; i++){
                dp[i] = new int[kapasite + 1];
            }

            for (int i = 0; i < urunSayisi + 1; i++){

                for (int j = 0; j < kapasite + 1; j++){

                    if (i == 0 || j == 0){
                        dp[i][j] = 0;
                    }else if (urunler[i - 1].agirlik <= j){
                        dp[i][j] = Math.Max(dp[i - 1][j], dp[i - 1][j - urunler[i - 1].agirlik] + urunler[i - 1].fiyat);
                    }else{
                        dp[i][j] = dp[i - 1][j];
                    }

                }
            }

            return dp[urunSayisi][kapasite];
        }

        // Bu fonksiyon belirtilen dosyadan öğeleri okur ve elde edilebilecek toplam fiyatı hesaplar.
        static void Main(string[] args){

            // Öğeleri belirtilen dosyadan okur ve kapasiteyi ve öğeleri döndürür.
            var (kapasite, urunler) = UrunleriOku("items.txt");
            UrunleriYazdir(kapasite, urunler);

            // Toplam fiyatı hesaplar
            var toplamFiyat = Canta(kapasite, urunler);
            Console.WriteLine("\nToplam Fiyat: " + toplamFiyat + " TL");
            
        }
        
    }
    
}
