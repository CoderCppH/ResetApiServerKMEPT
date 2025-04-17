package com.example.myapplication

import android.app.Dialog
import android.content.Intent
import android.os.Bundle
import android.util.Log
import android.view.View
import android.widget.Button
import android.widget.EditText
import android.widget.ImageView
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import com.example.myapplication.ACTIVITY.MainMenuActivity
import com.example.myapplication.API.ApiClient
import com.example.myapplication.CONFIG_USER.ConfigUser
import com.example.myapplication.CONFIG_USER.sql_p_user
import com.example.myapplication.GL.GL
import com.example.myapplication.MAILSS.MailSs
import com.example.myapplication.SETUP.SetUp
import com.google.gson.Gson
import kotlin.concurrent.thread
import kotlin.random.Random

class MainActivity : AppCompatActivity() {
    lateinit var img:ImageView
    lateinit var et_email:EditText
    lateinit var g_code:String
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        Init();
    }
    private fun Init() {
        SetUp(this)
        img = findViewById(R.id.img_V)
        loop_rotation_img()
        et_email = findViewById(R.id.activity_main_edit_text_email)
        var user = ConfigUser(this).get_user();
        if(user.id >= 0) {
            next_activity()
        }

    }
    private fun loop_rotation_img() {
        thread(){
            var y:Double = 0.0
            while (true)
            {
                rotation_img(y)
                y++
                Thread.sleep(20)
            }
        }
    }
    private fun rotation_img(y_float:Double)  {
        img.rotationY = y_float.toFloat();
        img.rotationX = y_float.toFloat();
    }
    private fun generatorCode():String {
        var code = ""
        var alpha = "QWERTYUIOPASDFGHJKLZXCVBNM1234567890"
        for (i:Int in 0..5) {
            var c_rand_alpha = alpha[Random.nextInt(0, alpha.length-1)]
            code+= c_rand_alpha;
        }
        return code
    }
    fun check_gmail_index(email:String):Boolean {
        return android.util.Patterns.EMAIL_ADDRESS.matcher(email.toString()).matches()
    }
    private fun next_activity() {
        finishAffinity()
        var intent = Intent(this, MainMenuActivity::class.java)
        startActivity(intent)
    }
    public fun on_click_go_to(view:View) {
        var email_string = et_email.text.toString()
        if(check_gmail_index(email_string)) {
            thread () {
                var email_sender = MailSs()
                g_code = generatorCode()
                var body_text = "только ни кому не сообщай об этом коде !!! \n code: ${g_code}"
                var head_text = "очень важный код от ChatLink"
                runOnUiThread {
                    Toast.makeText(this, email_string, Toast.LENGTH_SHORT).show()
                }
                email_sender.sendMessage(body_text, head_text, email_string);
                var code:EditText = EditText(this);
                var btn:Button = Button(this);
                runOnUiThread {
                    var dialog: Dialog = Dialog(this);
                    dialog.setTitle("CODE");
                    dialog.setContentView(R.layout.dialog_activity);
                    code = dialog.findViewById(R.id.dialog_edit_text_code)
                    btn = dialog.findViewById(R.id.dialog_confirm_code_btn)
                    dialog.show()
                }
                runOnUiThread {
                    btn.setOnClickListener({
                        //Log.d("input_code", code.text.toString() + ", " + g_code.toString())
                        if (code.text.toString().equals(g_code)) {
                            Log.d("input_code", "SUCCESS")

                            var config_user = ConfigUser(this);
                            var p_user =
                                sql_p_user();
                            p_user.email = email_string
                            p_user.id = 0;
                            p_user.full_name = "user_" + g_code

                            var gson = Gson();

                            var api = ApiClient();

                            var res = api.POST(
                                GL.url_api_server + "make_user",
                                gson.toJson(p_user)
                            );

                            if (res.equals("{\"code_status\":true}")) {
                                config_user.edit_config_user(p_user)
                                next_activity()
                            }
                            else {

                                config_user.edit_config_user(p_user)
                                next_activity()

                                res = api.POST(
                                    GL.url_api_server + "login_user",
                                    gson.toJson(p_user)
                                );

                                if (res.equals("{\"code_status\":true}")) {

                                    config_user.edit_config_user(p_user)
                                    next_activity()

                                } else {
                                    Toast.makeText(
                                        this,
                                        "хмм где-то ошибка: " + res.toString(),
                                        Toast.LENGTH_SHORT
                                    ).show();

                                }
                            }
                        }
                    })
                }
            }
        }
    }
}