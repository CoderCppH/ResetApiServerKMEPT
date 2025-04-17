package com.example.myapplication.ACTIVITY

import android.content.Intent
import android.os.Bundle
import android.util.Log
import android.view.Menu
import android.view.MenuItem
import android.view.View
import android.widget.TextView
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.fragment.app.Fragment
import com.example.myapplication.CONFIG_USER.ConfigUser
import com.example.myapplication.R
import com.example.myapplication.SETUP.SetUp
import com.google.android.material.bottomnavigation.BottomNavigationView
import com.google.gson.Gson

class MainMenuActivity : AppCompatActivity() {
    lateinit var user_config:ConfigUser;
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.main_menu_activity)
        Init();

    }

    override fun onCreateOptionsMenu(menu: Menu?): Boolean {
        menuInflater.inflate(R.menu.menu_for_main_menu_activity, menu)
        return true
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        return when (item.itemId)
        {
            R.id.menu_for_main_menu_activity_item_Setings-> {
                Log.d("main_meni_MENU", "settings")
                var settings_activity_i = Intent(this, SettingsActivity::class.java)
                startActivity(settings_activity_i)
                true
            }
            R.id.menu_for_main_menu_activity_item_params1-> {
                Log.d("main_meni_MENU", "params1")
                true
            }
            R.id.menu_for_main_menu_activity_item_params2-> {
                Log.d("main_meni_MENU", "params2")
                true
            }
            else-> super.onOptionsItemSelected(item)
        }

    }
    private fun Init() {
        SetUp(this)
        setSupportActionBar(findViewById(R.id.toolbar))
        user_config = ConfigUser(this);
        Log.d("user_info", Gson().toJson(user_config.get_user()))
        Toast.makeText(this, Gson().toJson(user_config.get_user()) , Toast.LENGTH_SHORT).show();
    }
}