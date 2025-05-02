package com.example;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.io.LineNumberReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

public class Main {
    public static void main(String[] args) {
        System.out.println("Start");
        Gson gson = new Gson();
        String url = "http://185.159.129.187:5226/api/messagers/9/11";

        ApiClient api = new ApiClient();
        var type = new TypeToken<ArrayList<Message>>(){}.getType();
        var request = api.GET(url);
        System.out.println(request);
        if(request.length() > 3){
            ArrayList<Message> list_message = gson.fromJson(request, type);
            for (var item : list_message) {
                System.out.println(item.message);
            }

        }
        
        
    }
}