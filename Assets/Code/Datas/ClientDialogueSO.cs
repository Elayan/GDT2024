using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "ClientDialogue", menuName ="Client/Dialogue")]
public class ClientDialogueSO : ScriptableObject
{
    [SerializeField]
    private List<string> _helloPhrases;
    [SerializeField]
    private List<string> _winPhrases;
    [SerializeField]
    private List<string> _loosePhrase;

    public List<string> HelloPhrases { get => _helloPhrases; }
    public List<string> WinPhrases { get => _winPhrases; }
    public List<string> LoosePhrases { get => _loosePhrase; }
}
