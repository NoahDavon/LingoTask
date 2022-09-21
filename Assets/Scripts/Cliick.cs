using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;
using System.Text.RegularExpressions;
public class Cliick : MonoBehaviour, IPointerClickHandler
{
    TMP_Text textObject;
    List<int> wordLanguageCodes;
    int targetLanguage = 1;
    int defaultLanguage = 0;
    void Start()
    {
        textObject = gameObject.GetComponent<TMP_Text>();

    }
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if(wordLanguageCodes is null) wordLanguageCodes = Enumerable.Repeat(defaultLanguage, textObject.textInfo.wordCount).ToList();
        int index = TMP_TextUtilities.FindIntersectingWord(textObject, pointerEventData.position, null);
        if(index == -1) return;
        var word = textObject.textInfo.wordInfo[index].GetWord();
        var rgx = new Regex("(<[^<>]+>)?"+word+"(</[^<>]+>)?");
        Debug.Log(word);
        var isDefaultLanguage = wordLanguageCodes[index] == defaultLanguage;
        var translatedWord = 
        textObject.text = rgx.Replace(textObject.text, Translate(word, isDefaultLanguage? defaultLanguage:targetLanguage, isDefaultLanguage? targetLanguage : defaultLanguage));
        wordLanguageCodes[index] = isDefaultLanguage? targetLanguage : defaultLanguage;
    }

    string Translate(string word, int languageCode, int targetLanguageCode)
    {
        var wasCapitalized = char.IsUpper(word[0]);
        word = word.ToLower();
        var dictionaries = new List<Dictionary<string,string>>(){
            new Dictionary<string,string>(){
                {"hello", "salut"},
                {"my", "mon"},
                {"friend", "ami"}
            },
            new Dictionary<string,string>(){
                {"salut", "hello"},
                {"mon", "my"},
                {"ami", "friend"}
            }
        };
        var colorTags = new List<string>(){
            "[WORD]", 
            "<color=red>[WORD]</color=red>"
        };
        var translatedWord = dictionaries[languageCode][word];
        if(wasCapitalized) translatedWord = char.ToUpper(translatedWord[0]) + translatedWord.Substring(1);
        return colorTags[targetLanguageCode].Replace("[WORD]", translatedWord);
    }
}
