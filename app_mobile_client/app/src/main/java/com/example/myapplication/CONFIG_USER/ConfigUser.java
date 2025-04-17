package com.example.myapplication.CONFIG_USER;

import android.content.Context;
import android.util.Log;

import com.google.gson.Gson;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.FileReader;
import java.io.IOException;

public class ConfigUser {
    public static final String ConfigPath = "config.cnf";
    private  Context context;
    private File file;
    public ConfigUser(Context context) {
        this.context = context;
        file = new File(context.getFilesDir(), ConfigPath);
    }
    public sql_p_user get_user() {
        sql_p_user user = new sql_p_user();
        if(file != null && file.exists()) {
            String json;
            try {
                BufferedReader reader = new BufferedReader(new FileReader(file));
                json = reader.readLine();
            } catch (FileNotFoundException e) {
                throw new RuntimeException(e);
            } catch (IOException e) {
                throw new RuntimeException(e);
            }
            Gson gson = new Gson();
            user = gson.fromJson(json, sql_p_user.class);
        }
        else {
            user.id = -1;
            user.email = "NPU";
            user.full_name = "NFNL";
            try {
                file.createNewFile();
                edit_config_user(user);
            } catch (IOException e) {
                Log.d("config_user", e.getMessage());
            }
        }
        return user;
    }
    public void edit_config_user(sql_p_user user) {
        try {
            FileOutputStream fileOutputStream = context.openFileOutput(ConfigPath, Context.MODE_PRIVATE);
            Gson gson = new Gson();
            fileOutputStream.write(gson.toJson(user).getBytes());
            fileOutputStream.close();
        } catch (IOException e) {
            Log.d("config_user", e.getMessage());
        }
    }
}
