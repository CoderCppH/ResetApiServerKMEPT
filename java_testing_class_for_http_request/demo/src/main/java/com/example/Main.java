package com.example;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;

import com.google.gson.Gson;

public class Main {
    public static void main(String[] args) {
        System.out.println("Start");
        String url = "http://127.0.0.1:5226/api/users/1";

        ApiClient api = new ApiClient();
        
        System.out.println(api.GET(url));
        
    }
}