//ToDo:
/*
   Scrolling text(text scrolls when screen is full)
   Commands

*/
#include <Adafruit_TFTLCD.h>
#include <Adafruit_GFX.h>
#include <TouchScreen.h>

#define LCD_CS A3
#define LCD_CD A2
#define LCD_WR A1
#define LCD_RD A0
#define LCD_RESET A4

#define TS_MINX 204
#define TS_MINY 195
#define TS_MAXX 948
#define TS_MAXY 910

#define YP A2  // must be an analog pin, use "An" notation!
#define XM A3  // must be an analog pin, use "An" notation!
#define YM 8   // can be a digital pin
#define XP 9   // can be a digital pin

#define BLACK   0x0000
#define BLUE    0x001F
#define RED     0xF800
#define GREEN   0x07E0
#define CYAN    0x07FF
#define MAGENTA 0xF81F
#define YELLOW  0xFFE0
#define WHITE   0xFFFF

bool recieveInput = true;
int commandCursorY = 219;

Adafruit_TFTLCD tft(LCD_CS, LCD_CD, LCD_WR, LCD_RD, LCD_RESET);
TouchScreen ts = TouchScreen(XP, YP, XM, YM, 300);
String commandString = "> ";
void initScreen() {
  tft.reset();
  tft.begin(0x9325);
  tft.setAddrWindow(0, 0, 240, 320);
  tft.setRotation(1);
  tft.setTextSize(1);
  
  tft.fillScreen(BLACK);
  //Boot  sequence
  recieveInput=false;
  String bootText="WELCOME TO ROVER OS v.2.3.2. LINKING WITH ROVER NOW...";
  String bootText2="CONNECTED TO ROVER. FOR A LIST OF COMMANDS TYPE HELP() (NON-CASE SENSITIVE)";
  for(int i=0; i<bootText.length(); i++){
    tft.print(bootText.charAt(i));
    delay(100);
  }
  delay(1000);
  tft.setTextColor(BLACK);
  tft.setCursor(0,0);
  
  tft.print(bootText);
  tft.setTextColor(WHITE);
  for(int i=0; i<bootText2.length(); i++){
    tft.print(bootText2.charAt(i));
    delay(100);
  }
  delay(1000);
  tft.setCursor(0,0);
  tft.setTextColor(BLACK);
  tft.print(bootText2);
  recieveInput=true;
  clearScreen();
}

void clearScreen() {
  tft.fillScreen(BLACK);
  commandString = "> ";
  tft.setTextColor(WHITE);
  tft.setCursor(0,commandCursorY);
  tft.print(commandString);
}

void SendCommand(String command){
    commandString="Sending command...";
    tft.setTextColor(WHITE);
    tft.setCursor(0, commandCursorY);
    tft.print(commandString);
    delay(1000);
    Serial.println("cmd: "+command);
    tft.setTextColor(BLACK);
    tft.setCursor(0, commandCursorY);
    tft.print(commandString);
    commandString="> ";
    tft.setTextColor(WHITE);
    tft.setCursor(0,commandCursorY);
    tft.print(commandString);
    recieveInput=true;
}

void updateCommandString(String c) {
  //Set color black
  tft.setTextColor(BLACK);
  //'Overwrite' the current white text of the command (effectively erasing it). This is more efficient than using fillScreen because it's much quicker
  tft.setCursor(0, commandCursorY);
  tft.print(commandString);
  if(c!="\b" && c!="\r" && c!="sendcommand"){
    commandString.concat(c);
  } else if(c=="\b" && commandString.length()>2) {
    commandString.remove(commandString.length()-1);
  } else if (c=="\r"){
    recieveInput=false;
    String c=commandString;
    String command =commandString.substring(2);
    command.toLowerCase();
    SendCommand(command);
    return;
  }
  //Write the new command in white text.
  tft.setTextColor(WHITE);
  tft.setCursor(0, commandCursorY);
  tft.print(commandString);
}



void setup() {
  Serial.begin(9600);
  initScreen();
  Serial.setTimeout(10);
  
}


void loop() {
  if(Serial.available()>0 && recieveInput){
      updateCommandString(Serial.readString().substring(0,1));  
  }
}
