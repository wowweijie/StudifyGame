using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Result
{
    public string _id;
    public string userId;
    public List<string> intro;
    public List<string> re;
    public List<string> sd;
    public List<string> sv;
    public List<string> sm;
    public float __v;
    public float introScore;
    public float reScore;
    public float sdScore;
    public float svScore;
    public float smScore;
    public float totalScore;
    public Result(string id, string userId, List<string> intro, List<string> re, List<string> sd, List<string> sv, List<string> sm, float v, float introScore, float reScore, float sdScore, float svScore, float smScore)
    {
        _id = id;
        this.userId = userId;
        this.intro = intro;
        this.re = re;
        this.sd = sd;
        this.sv = sv;
        this.sm = sm;
        __v = v;
        this.introScore = introScore;
        this.reScore = reScore;
        this.sdScore = sdScore;
        this.svScore = svScore;
        this.smScore = smScore;
        this.totalScore = 0;
    }

    public void computeTotalScore()
    {
        this.totalScore = (this.introScore + this.reScore + this.sdScore + this.svScore + this.smScore) * 7;
    }
}
