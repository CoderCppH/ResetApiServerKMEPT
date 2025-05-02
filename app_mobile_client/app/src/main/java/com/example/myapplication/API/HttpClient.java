package com.example.myapplication.API;

import android.util.Log;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.ProtocolException;
import java.net.URL;

public class HttpClient {

    public String POST(String url, String json) {
        return executeRequest(url, "POST", json);
    }

    public String PUT(String url, String json) {
        return executeRequest(url, "PUT", json);
    }

    public String DELETE(String url) {
        return executeRequest(url, "DELETE", null);
    }

    public String GET(String url) {
        HttpURLConnection connection = null;
        try {
            URL obj = new URL(url);
            connection = (HttpURLConnection) obj.openConnection();
            connection.setRequestMethod("GET");
            connection.setRequestProperty("User-Agent", "Mozilla/5.0");

            int responseCode = connection.getResponseCode();
            Log.d("RESPONSE_CODE", String.valueOf(responseCode));

            if (responseCode == HttpURLConnection.HTTP_OK) {
                return readResponse(connection);
            } else {
                Log.d("API.GET.ERROR", "Response Code: " + responseCode);
            }
        } catch (Exception ex) {
            Log.d("API.GET.ERROR", ex.getMessage());
        } finally {
            if (connection != null) {
                connection.disconnect();
            }
        }
        return "error_get";
    }

    private String executeRequest(String url, String method, String json) {
        HttpURLConnection connection = null;
        try {
            URL obj = new URL(url);
            connection = (HttpURLConnection) obj.openConnection();
            connection.setRequestMethod(method);
            connection.setRequestProperty("Content-Type", "application/json");
            connection.setRequestProperty("Accept", "application/json");
            connection.setDoOutput(true);

            if (json != null) {
                try (OutputStream os = connection.getOutputStream()) {
                    byte[] input = json.getBytes("utf-8");
                    os.write(input, 0, input.length);
                }
            }

            int responseCode = connection.getResponseCode();
            Log.d("RESPONSE_CODE", String.valueOf(responseCode));

            return readResponse(connection);
        } catch (Exception ex) {
            Log.d("API." + method + ".ERROR", ex.getMessage());
        } finally {
            if (connection != null) {
                connection.disconnect();
            }
        }
        return "error_" + method.toLowerCase();
    }

    private String readResponse(HttpURLConnection connection) throws IOException {
        StringBuilder response = new StringBuilder();
        try (BufferedReader in = new BufferedReader(new InputStreamReader(connection.getInputStream()))) {
            String inputLine;
            while ((inputLine = in.readLine()) != null) {
                response.append(inputLine);
            }
        }
        return response.toString();
    }
}