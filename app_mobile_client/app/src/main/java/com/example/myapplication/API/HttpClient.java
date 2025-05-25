package com.example.myapplication.API;

import android.util.Log;

import java.io.IOException;
import java.util.concurrent.TimeUnit;

import okhttp3.MediaType;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;
import okhttp3.logging.HttpLoggingInterceptor;

public class HttpClient {
    private static final MediaType JSON = MediaType.parse("application/json; charset=utf-8");
    private final OkHttpClient client;

    public HttpClient() {
        // Настройка логгирования (только для debug)
        HttpLoggingInterceptor loggingInterceptor = new HttpLoggingInterceptor();
        loggingInterceptor.setLevel(HttpLoggingInterceptor.Level.BODY);

        // Создаем клиент с настройками таймаутов
        this.client = new OkHttpClient.Builder()
                .connectTimeout(60, TimeUnit.SECONDS)  // Таймаут подключения
                .readTimeout(60, TimeUnit.SECONDS)     // Таймаут чтения
                .writeTimeout(60, TimeUnit.SECONDS)    // Таймаут записи
                .addInterceptor(loggingInterceptor)     // Логгирование запросов/ответов
                .build();
    }

    public String POST(String url, String json) throws IOException {
        RequestBody body = RequestBody.create(json, JSON);
        Request request = new Request.Builder()
                .url(url)
                .post(body)
                .build();

        return executeRequest(request);
    }

    public String PUT(String url, String json) throws IOException {
        RequestBody body = RequestBody.create(json, JSON);
        Request request = new Request.Builder()
                .url(url)
                .put(body)
                .build();

        return executeRequest(request);
    }

    public String DELETE(String url) throws IOException {
        Request request = new Request.Builder()
                .url(url)
                .delete()
                .build();

        return executeRequest(request);
    }

    public String GET(String url) throws IOException {
        Request request = new Request.Builder()
                .url(url)
                .get()
                .build();

        return executeRequest(request);
    }

    private String executeRequest(Request request) throws IOException {
        try (Response response = client.newCall(request).execute()) {
            if (!response.isSuccessful()) {
                throw new IOException("HTTP error code: " + response.code() +
                        ", message: " + response.message());
            }

            String responseBody = response.body() != null ? response.body().string() : "";
            Log.d("HTTP_RESPONSE", "Response from " + request.method() + " " + request.url() +
                    ": " + responseBody);

            return responseBody;
        } catch (IOException e) {
            Log.e("HTTP_ERROR", "Error in " + request.method() + " " + request.url(), e);
            throw e;
        }
    }
}