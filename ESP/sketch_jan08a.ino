/***************************************************************************  

  // + Humidity     4b4e6872-9ca7-4616-97fc-521f9308deb9
  // + Temperature  28d53665-9ba3-4769-bc32-882d710123fb
  // + IP           c4d34d90-a68e-4de0-a98b-ac2cd7ef9f09
  // + Fire         b09e04bc-b92c-49d1-adb9-6984d6a31e92
  // + Movements    89b62e85-b6c3-4816-b692-ce2fc00b08ae
  // + WiFiRSSI     1b4af968-38db-44ce-81dd-c7a2615a3761
  // + Vibration    76711a7c-2fed-4376-83d3-1c00a490792e
  
  // + Beep         0f94812c-2501-4a00-987d-10eb7111873e
  // + Lamp         ed14d1d7-e79f-4b7d-9a2c-8efd38845fd9
  // - Fan          fc37f8d2-ce62-4788-a186-e5bcd741db87
  
  
 ***************************************************************************/

#include <ESP8266WiFi.h>
#include <Wire.h>
#include <SPI.h>
#include <ESP8266HTTPClient.h>
#include "DHT.h"

#define DHTTYPE DHT11 // DHT 11

// Pins
int ledPin = 2;
int flamePin = 12;
int movePin = 4;
int tonePin = 5;
int lampPin = 14;
int vibrationPin = 15;
int dhtPin = 2;
int fanPin = 13;

DHT dht(dhtPin, DHTTYPE);

// Results
float t = 0;
float h = 0;
float p = 0;
int f = 0;
int m = 0;
int v = 0;
long r = 0;
IPAddress localIP;

// WiFi
struct { 
    char ssid[20] = "";
    char passw[10] = "";
    char device[20] = "";
} data;

bool connectToWiFi() {
    WiFi.begin(data.ssid, data.passw); //try to connect with wifi
    Serial.print("Connecting to ");
    Serial.print(data.ssid);
    while (WiFi.status() != WL_CONNECTED) {
        Serial.print(".");
        delay(500);
    }
    Serial.println();
    Serial.print("Connected to ");
    Serial.println(data.ssid);
    Serial.print("IP Address is : ");
    localIP = WiFi.localIP();
    Serial.println(localIP); //print local IP address
    Serial.println();
    return true;
}

bool readData() {
    // Humidity & Temperature
    h = dht.readHumidity();
    t = dht.readTemperature();
    Serial.println("");
    Serial.print(t);
    Serial.print(" *C \n");
    Serial.print(h);
    Serial.print(" %");

    // Fire
    if(!digitalRead(flamePin)) {
        f = 1;
        Serial.println("\nFire");
    } else {
        f = 0;
        Serial.println("\nNo Fire");
    }

    // Move
    m = digitalRead(movePin);
    Serial.println(String(m) + " move");
    
    // Wifi RSSI
    r = WiFi.RSSI();
    Serial.print(r);
    Serial.print(" dBm\n");

    // Hit
    v = digitalRead(vibrationPin);
    Serial.println(String(v) + " vibration");
    
    return true;
}

bool postData() {
    if (WiFi.status() == WL_CONNECTED) { //Check WiFi connection status
        HTTPClient http;  //Declare an object of class HTTPClient
        String url = "";
        int httpCode = 0;
        digitalWrite(ledPin, LOW);

        // Temperature
        url = "http://138.201.188.137:8080/api/Value/getdata?token=28d53665-9ba3-4769-bc32-882d710123fb&value=" + String(t);
        http.begin(url);
        httpCode = http.GET();
        http.end();        
        Serial.print("HTTP GET (" + url + "): " + httpCode);
        Serial.println();

        // Humidity
        url = "http://138.201.188.137:8080/api/Value/getdata?token=4b4e6872-9ca7-4616-97fc-521f9308deb9&value=" + String(h);
        http.begin(url);
        httpCode = http.GET();
        http.end();        
        Serial.print("HTTP GET (" + url + "): " + httpCode);
        Serial.println();

        // IP
        url = "http://138.201.188.137:8080/api/Value/getdata?token=c4d34d90-a68e-4de0-a98b-ac2cd7ef9f09&value=" + localIP.toString();
        http.begin(url);
        httpCode = http.GET();
        http.end();        
        Serial.print("HTTP GET (" + url + "): " + httpCode);
        Serial.println();

        // Fire
        url = "http://138.201.188.137:8080/api/Value/getdata?token=b09e04bc-b92c-49d1-adb9-6984d6a31e92&value=" + String(f);
        http.begin(url);
        httpCode = http.GET();
        http.end();        
        Serial.print("HTTP GET (" + url + "): " + httpCode);
        Serial.println();

        // Move
        url = "http://138.201.188.137:8080/api/Value/getdata?token=89b62e85-b6c3-4816-b692-ce2fc00b08ae&value=" + String(m);
        http.begin(url);
        httpCode = http.GET();
        http.end();        
        Serial.print("HTTP GET (" + url + "): " + httpCode);
        Serial.println();

        // Wifi RSSI
        url = "http://138.201.188.137:8080/api/Value/getdata?token=1b4af968-38db-44ce-81dd-c7a2615a3761&value=" + String(r);
        http.begin(url);
        httpCode = http.GET();
        http.end();        
        Serial.print("HTTP GET (" + url + "): " + httpCode);
        Serial.println();

        // Vibration
        url = "http://138.201.188.137:8080/api/Value/getdata?token=76711a7c-2fed-4376-83d3-1c00a490792e&value=" + String(v);
        http.begin(url);
        httpCode = http.GET();
        http.end();        
        Serial.print("HTTP GET (" + url + "): " + httpCode);
        Serial.println();

        digitalWrite(ledPin, HIGH);
    } else {
        Serial.println("WiFi connection error");
        return false;
    }

    return true;
}

bool getData() {
    if (WiFi.status() == WL_CONNECTED) { //Check WiFi connection status
        HTTPClient http;  //Declare an object of class HTTPClient
        String url = "";
        String res = "";
        int httpCode = 0;
        digitalWrite(ledPin, LOW);

        // Fan
        url = "http://138.201.188.137:8080/api/Value/getaction?token=fc37f8d2-ce62-4788-a186-e5bcd741db87";
        http.begin(url);
        httpCode = http.GET();
        if (httpCode > 0) {
            res = http.getString();
            if (res == "1") {
                digitalWrite(fanPin, HIGH); // ON            
            } else {
                digitalWrite(fanPin, LOW); // OFF
            }
        }
        http.end();        
        Serial.print("HTTP GET (" + url + "): " + res);
        Serial.println();
        
        
        // Lamp
        url = "http://138.201.188.137:8080/api/Value/getaction?token=0f94812c-2501-4a00-987d-10eb7111873e";
        http.begin(url);
        httpCode = http.GET();
        if (httpCode > 0) {
            res = http.getString();
            if (res == "1") {
                digitalWrite(lampPin, HIGH); // ON            
            } else {
                digitalWrite(lampPin, LOW); // OFF
            }
        }
        http.end();        
        Serial.print("HTTP GET (" + url + "): " + res);
        Serial.println();

        // Beep
        url = "http://138.201.188.137:8080/api/Value/getaction?token=ed14d1d7-e79f-4b7d-9a2c-8efd38845fd9";
        http.begin(url);
        httpCode = http.GET();
        if (httpCode > 0) {
            res = http.getString();
            if (res == "1") {
                tone(tonePin, 2000, 500);
                delay(1000);
                tone(tonePin, 2000, 500);
                delay(1000);
                tone(tonePin, 2000, 500);
            }
        }
        http.end();        
        Serial.print("HTTP GET (" + url + "): " + res);
        Serial.println();
   
        http.end();   //Close connection
    }
    
    digitalWrite(ledPin, HIGH);
    return true;
}

void setup() {
    Serial.begin(9600);
    Serial.println("ESP Started");
    connectToWiFi();
    
    pinMode(ledPin, OUTPUT);
    pinMode(fanPin, OUTPUT);
    pinMode(lampPin, OUTPUT);
    digitalWrite(ledPin, HIGH);

    dht.begin();

    pinMode(flamePin, INPUT);
    pinMode(movePin, INPUT);
    pinMode(vibrationPin, INPUT);
}

void loop() { 
    if (!readData()) return;
    if (!postData()) return;
    if (!getData()) return;

    Serial.print("\nWait [");
    for (int i = 0; i < 2; i++) {
        Serial.print(".");
        delay(1000);
    }
    Serial.print("]\n");
}
