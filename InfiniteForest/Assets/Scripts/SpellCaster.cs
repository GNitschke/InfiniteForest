using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    float maxMana;
    float mana;


    public void CastSpell(List<int> _incantationList)
    {
        int _incantation = _incantationList[_incantationList.Count - 1];
        for (int i = _incantationList.Count - 2; i >= 0; i--)
        {
            _incantation *= 10;
            _incantation += _incantationList[i];
        }
        CastSpell(_incantation);
    }

    public void CastSpell(int _incantation)
    {
        if(_incantation > 0)
        {
            int _step = _incantation % 10;
            _incantation /= 10;

            switch (_step)
            {
                case 1:
                    if (_incantation <= 0)
                    {
                        Debug.Log("Slow");
                    }
                    else
                    {
                        _step = _incantation % 10;
                        _incantation /= 10;

                        switch (_step)
                        {
                            case 2:
                                Debug.Log("Other bubble");
                                break;
                            case 3:
                                Debug.Log("Other Glow");
                                break;
                            case 4:
                                Debug.Log("Fizzle");
                                break;
                            default:
                                Debug.LogError("Invalid Spell");
                                break;
                        }
                    }
                    break;
                case 2:
                    if (_incantation <= 0)
                    {
                        Debug.Log("Force");
                    }
                    else
                    {
                        _step = _incantation % 10;
                        _incantation /= 10;

                        switch (_step)
                        {
                            case 1:
                                Debug.Log("Shield");
                                break;
                            case 3:
                                Debug.Log("Sparks");
                                break;
                            case 4:
                                Debug.Log("Fizzle");
                                break;
                            default:
                                Debug.LogError("Invalid Spell");
                                break;
                        }
                    }
                    break;
                case 3:
                    if (_incantation <= 0)
                    {
                        Debug.Log("Light");
                    }
                    else
                    {
                        _step = _incantation % 10;
                        _incantation /= 10;

                        switch (_step)
                        {
                            case 1:
                                Debug.Log("Spawn Light");
                                break;
                            case 2:
                                Debug.Log("Fizzle");
                                break;
                            case 4:
                                Debug.Log("Follow Light");
                                break;
                            default:
                                Debug.LogError("Invalid Spell");
                                break;
                        }
                    }
                    break;
                case 4:
                    if (_incantation <= 0)
                    {
                        Debug.Log("Energize");
                    }
                    else
                    {
                        _step = _incantation % 10;
                        _incantation /= 10;

                        switch (_step)
                        {
                            case 1:
                                Debug.Log("Fizzle");
                                break;
                            case 2:
                                Debug.Log("Self Bubble");
                                break;
                            case 3:
                                Debug.Log("Self Glow");
                                break;
                            default:
                                Debug.LogError("Invalid Spell");
                                break;
                        }
                    }
                    break;
                default:
                    Debug.LogError("Invalid Spell");
                    break;
            }
        }
    }
}
