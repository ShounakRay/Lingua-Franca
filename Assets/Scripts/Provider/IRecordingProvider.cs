using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IRecordingProvider
{
    void StartRecording();
    Task<string> StopRecording();
}

public class DummyRecordingProvider : IRecordingProvider
{
    private static string[] responses = {
        "Hey! I'm doing great, thanks for asking!",
        "Good morning! I hope you're having a fantastic day!",
        "Hi there! I'm doing well, how about you?",
        "Hello! Always nice to hear from you!",
        "Not much, just chilling. What about you?",
        "Hey! Things are going pretty good, how about yourself?",
        "Morning! Ready to conquer the day?",
        "Hiya! How's your day shaping up?",
        "Yo! What's the latest?",
        "Hi! Haven't seen you around here before, what's your story?",
        "Howdy! What's been happening in your world?",
        "Hey! It's been too long, we should catch up!",
        "Greetings! What brings you here today?",
        "Hey! Anything exciting happening in your neck of the woods?",
        "Well, hello there! What's new with you?",
        "Hey! Feels like ages since we last spoke, how have you been?",
        "Hey buddy! How's life treating you?",
        "Hi! Pleasure to meet you too!",
        "Hello! I've been doing pretty well, thanks for asking!",
        "Hey! My day's been pretty good, how about yours?",
        "Good day! What's on your agenda?",
        "Hi! Everything's been going smoothly, thanks for asking!",
        "Hello! Haven't seen you around lately, what's new?",
        "Hey! I've been doing alright, how about yourself?",
        "Hi! Life's been treating me pretty well, how about you?"
    };

    private float startTime;

    public void StartRecording() 
    {
        startTime = Time.time;
    }

    public async Task<string> StopRecording()
    {
        if (Time.time - startTime < 1f) return null; 
        await Task.Delay(Random.Range(100, 1000)); // Simulate latency
        return responses[Random.Range(0, responses.Length)];
    }
}