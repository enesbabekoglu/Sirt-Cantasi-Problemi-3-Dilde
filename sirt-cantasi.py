import sys
from typing import List, Tuple

def UrunleriOku(filename):

    # Okunan dosyadaki öğeleri okur ve kapasiteyi ve öğeleri bir çift olarak döndürür

    with open(filename, "r") as f:

        kapasite = int(f.readline().strip())
        urunler = []

        for satir in f:
            agirlik, fiyat = map(int, satir.strip().split())
            urunler.append((agirlik, fiyat))

    return kapasite, urunler

def UrunleriSec(kapasite, urunler):
    
    # Burada maksimum fiyatı elde etmek için gereken en iyi ürün listesini seçer.
    
    urunSayisi = len(urunler)
    dp = [[0] * (kapasite + 1) for _ in range(urunSayisi + 1)]

    for i in range(urunSayisi + 1):
        for j in range(kapasite + 1):
            if i == 0 or j == 0:
                dp[i][j] = 0
            elif urunler[i - 1][0] <= j:
                dp[i][j] = max(dp[i - 1][j], dp[i - 1][j - urunler[i - 1][0]] + urunler[i - 1][1])
            else:
                dp[i][j] = dp[i - 1][j]

    secilenUrunler = []

    i, j = urunSayisi, kapasite
    while i > 0 and j > 0:
        if dp[i][j] != dp[i - 1][j]:
            secilenUrunler.append(urunler[i - 1])
            j -= urunler[i - 1][0]
        i -= 1

    return secilenUrunler

def UrunleriYazdir(kapasite, urunler):

    # Verilen kapasite ve öğeleri konsola yazdırır

    print("Canta Kapasitesi:", kapasite, " KG")
    print("\nURUNLER")

    for i, (agirlik, fiyat) in enumerate(urunler):
        print(f"{i+1}. Urun - Agirlik: {agirlik} KG / Fiyat: {fiyat} TL")
        
    secilen_urunler = UrunleriSec(kapasite, urunler)
    
    print("\nCANTADAKI URUNLER")
    
    for urun in secilen_urunler:
        print(f"Agirlik: {urun[0]} KG / Fiyat: {urun[1]} TL")

def Canta(kapasite, urunler):

    # Verilen listeden öğeler seçilerek elde edilebilecek en yüksek değeri döndürür.

    urunSayisi = len(urunler)
    dp = [[0] * (kapasite + 1) for _ in range(urunSayisi + 1)]

    for i in range(urunSayisi + 1):

        for j in range(kapasite + 1):

            if i == 0 or j == 0:
                dp[i][j] = 0
            elif urunler[i-1][0] <= j:
                dp[i][j] = max(dp[i-1][j], dp[i-1][j-urunler[i-1][0]] + urunler[i-1][1])
            else:
                dp[i][j] = dp[i-1][j]
    
    return dp[urunSayisi][kapasite]

def main():

    # Bu fonksiyon belirtilen dosyadan öğeleri okur ve elde edilebilecek toplam fiyatı hesaplar.

    # Öğeleri belirtilen dosyadan okur ve kapasiteyi ve öğeleri döndürür.
    kapasite, urunler = UrunleriOku("items.txt")
    UrunleriYazdir(kapasite, urunler)

    # Toplam fiyatı hesaplar
    toplamFiyat = Canta(kapasite, urunler)
    print("\nToplam Fiyat:", toplamFiyat, "TL")

main()

