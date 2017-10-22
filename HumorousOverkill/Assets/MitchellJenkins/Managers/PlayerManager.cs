﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct PlayerInfo {
    public int m_playerHealth;
    public float m_playerSpeed;
    public float m_playerJumpHeight;
    public int m_pickupHealthAmount;
    public int m_pickupAmmoAmount;
    public float m_gunFireRate_type1;
    public float m_gunFireRate_type2;
    public float m_gunDamage_type1;
    public float m_gunDamage_type2;
    public int m_gunMaxAmmo;
}

public class PlayerManager : GameEventListener {

    
}
