package com.example.myapplication.ACTIVITY;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import com.example.myapplication.CONFIG_USER.ConfigUser;
import com.example.myapplication.MainActivity;
import com.example.myapplication.R;
import com.example.myapplication.SETUP.SetUp;

public class SettingsActivity extends AppCompatActivity {
    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_settings_for_main_activity);
        new SetUp(this);
    }
    public void btn_exit_clicked(View view) {
        ConfigUser user = new ConfigUser(this);
        Log.d("Config.User: Deleted: ", user.Delet().toString());
        finishAffinity();
        var intent = new Intent(this, MainActivity.class);
        startActivity(intent);
    }
}
