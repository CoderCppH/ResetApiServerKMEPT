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
        String url_make_user = "http://127.0.0.1:8888/make_user";
        String url_login_user = "http://127.0.0.1:8888/login_user";
        sql_p_user user = new sql_p_user(0, "Pargev", "coder.cpp.h@gmail.com", "********");
        ApiClient api = new ApiClient();
        //System.out.println(api.POST(url_make_user, new Gson().toJson(user)));
        System.out.println(api.POST(url_login_user, new Gson().toJson(user)));
    }
}