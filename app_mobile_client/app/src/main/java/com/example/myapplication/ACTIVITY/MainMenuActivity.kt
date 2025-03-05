package com.example.myapplication.ACTIVITY

import android.os.Bundle
import android.util.Log
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import com.example.myapplication.CONFIG_USER.ConfigUser
import com.example.myapplication.CONFIG_USER.p_config_user
import com.example.myapplication.R
import com.example.myapplication.SETUP.SetUp
import com.google.gson.Gson
import java.io.File
import kotlin.coroutines.Continuation

class MainMenuActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.main_menu_activity)
        Init();

    }
    private fun Init() {
        SetUp(this)
        var user = ConfigUser(this);
        var p_user = p_config_user()
        p_user.id=0;
        p_user.email="pargev20002607@gmail.com"
        p_user.full_name="pargev just simpns"
        //user.edit_config_user(p_user)
        Log.d("user_info", Gson().toJson(user.get_user()))
        Toast.makeText(this, Gson().toJson(user.get_user()) , Toast.LENGTH_SHORT).show();
    }
}