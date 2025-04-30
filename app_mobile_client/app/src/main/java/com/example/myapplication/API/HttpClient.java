package com.example.myapplication.API;

import android.util.Log;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;
public class HttpClient {
    public String POST(String url, String json) {
        try {
            URL obj = new URL(url);
            HttpURLConnection objConnectURL = (HttpURLConnection) obj.openConnection();
            
            objConnectURL.setRequestMethod("POST");
            objConnectURL.setRequestProperty("Content-Type", "application/json");
            objConnectURL.setRequestProperty("Accept", "application/json");

            objConnectURL.setDoOutput(true);

            try(OutputStream os = objConnectURL.getOutputStream()) {
                byte [] input = json.getBytes("utf-8");
                os.write(input, 0, input.length);
            }

            BufferedReader in = new BufferedReader(new InputStreamReader(objConnectURL.getInputStream()));
            String inputLine;
            StringBuilder response = new StringBuilder();

            while ((inputLine = in.readLine()) != null) {
                response.append(inputLine);
            }
            in.close();
            return response.toString();
        } catch(Exception ex) {
            if(ex.getMessage() != null && ex.toString() != null) {
                Log.d("API.POST.ERROR", ex.getMessage());
            }
        }
        return "error";
    }
    public String GET(String url) {
        try {
            URL obj = new URL(url);
            HttpURLConnection objConnectURL = (HttpURLConnection) obj.openConnection();
            objConnectURL.setRequestMethod("GET");
            objConnectURL.setRequestProperty("User-Agent", "Mozilla/5.0");
            int responseCode = objConnectURL.getResponseCode();
            Log.d("RESPONSE_CODE",String.valueOf(responseCode));
            if (responseCode == HttpURLConnection.HTTP_OK) {
                BufferedReader in = new BufferedReader(new InputStreamReader(objConnectURL.getInputStream()));
                String inputLine;
                StringBuilder response = new StringBuilder();

                while((inputLine = in.readLine()) != null) {
                    response.append(inputLine);
                }
                in.close();
                return response.toString();
            }
        } catch(Exception ex) {
            Log.d("API.GET.ERROR", ex.getMessage());
        }
        return "error";
    }
}
