#include <iostream>
#include <fstream>
#include <vector>
#include <algorithm>

using namespace std;

pair<int, vector<pair<int, int>>> UrunleriOku(string filename){

	/* Okunan dosyadaki öğeleri okur ve kapasiteyi ve öğeleri bir çift olarak döndürür */

	int kapasite, agirlik, fiyat;

	vector<pair<int, int>> urunler;

	ifstream f(filename);
	f >> kapasite;

	while (f >> agirlik >> fiyat){
		urunler.push_back(make_pair(agirlik, fiyat));
	}

	return make_pair(kapasite, urunler);

}

vector<pair<int, int>> UrunleriSec(int kapasite, vector<pair<int, int>> urunler){
    
    // Burada maksimum fiyatı elde etmek için gereken en iyi ürün listesini seçer.
    
    int urunSayisi = urunler.size();
    vector<vector<int>> dp(urunSayisi + 1, vector<int>(kapasite + 1));

    for (int i = 0; i < urunSayisi + 1; i++){
        
        for (int j = 0; j < kapasite + 1; j++){
            
            if (i == 0 || j == 0){
                dp[i][j] = 0;
            }else if (urunler[i - 1].first <= j){
                dp[i][j] = max(dp[i - 1][j], dp[i - 1][j - urunler[i - 1].first] + urunler[i - 1].second);
            }else{
                dp[i][j] = dp[i - 1][j];
            }
            
        }
        
    }

    vector<pair<int, int>> secilenUrunler;

    for (int i = urunSayisi, j = kapasite; i > 0 && j > 0; i--){
        
        if (dp[i][j] != dp[i - 1][j]){
            
            secilenUrunler.push_back(urunler[i - 1]);
            j -= urunler[i - 1].first;
            
        }
        
    }

    return secilenUrunler;
    
}

void UrunleriYazdir(int kapasite, const vector<pair<int, int>>& urunler){

	/* Verilen kapasite ve öğeleri konsola yazdırır */

	cout << "Canta Kapasitesi: " << kapasite << " KG" << endl;
	cout << "\nURUNLER:" << endl;

	for (size_t i = 0; i < urunler.size(); i++){
		cout << i + 1 << ". Urun - Agirlik: " << urunler[i].first << " KG / Fiyat: " << urunler[i].second << " TL" << endl;
	}

    vector<pair<int, int>> secilenUrunler = UrunleriSec(kapasite, urunler);

    cout << "\nCANTADAKI URUNLER" << endl;
    
    for (auto urun : secilenUrunler){
        cout << "Agirlik: " << urun.first << " KG / Fiyat: " << urun.second << " TL" << endl;
    }

}

int Canta(int kapasite, const vector<pair<int, int>>& urunler){

	/* Verilen listeden öğeler seçilerek elde edilebilecek en yüksek değeri döndürür. */

	int urunSayisi = urunler.size();
	vector<vector<int>> dp(urunSayisi + 1, vector<int>(kapasite + 1));

	for (int i = 0; i <= urunSayisi; i++){

		for (int j = 0; j <= kapasite; j++){

			if (i == 0 || j == 0) {
				dp[i][j] = 0;
			}else if (urunler[i - 1].first <= j) {
				dp[i][j] = max(dp[i - 1][j], dp[i - 1][j - urunler[i - 1].first] + urunler[i - 1].second);
			}else {
				dp[i][j] = dp[i - 1][j];
			}
		}
	}

	return dp[urunSayisi][kapasite];

}

int main(){

	// Öğeleri verilen dosyadan okur ve kapasiteyi ve öğeleri döndürür

	pair<int, vector<pair<int, int>>> result = UrunleriOku("items.txt");
	int kapasite = result.first;
	vector<pair<int, int>> urunler = result.second;

	UrunleriYazdir(kapasite, urunler);

	// Toplam Fiyatı hesaplar

	int toplamFiyat = Canta(kapasite, urunler);
	cout << "\nToplam Fiyat: " << toplamFiyat << " TL" << endl;

	return 0;

}