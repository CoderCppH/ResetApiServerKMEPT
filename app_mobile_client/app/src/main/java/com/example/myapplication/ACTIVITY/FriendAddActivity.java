package com.example.myapplication.ACTIVITY;

import android.os.Bundle;
import android.util.Log;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.myapplication.API.ApiClient;
import com.example.myapplication.API.json_p_user;
import com.example.myapplication.GL.GL;
import com.example.myapplication.LIST_USER.UserAdapter;
import com.example.myapplication.LIST_USER.p_user_item;
import com.example.myapplication.R;
import com.example.myapplication.SETUP.SetUp;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.List;

public class FriendAddActivity extends AppCompatActivity {

    private RecyclerView recyclerView;
    private UserAdapter adapter;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_add_friend);
        new SetUp(this);
        recyclerView = findViewById(R.id.recycle_view_friends_on_add);
        recyclerView.setLayoutManager(new LinearLayoutManager(this));
        new Thread(()->{
            ArrayList<p_user_item> list_user_adapter = new ArrayList<>();
            ArrayList<json_p_user> list_user = new ArrayList<>();
            ApiClient api = new ApiClient();
            var temp = api.GET( GL.url_api_server  + "get_list_user");
            Gson gson = new Gson();
            Type userListType = new TypeToken<List<json_p_user>>() {}.getType();
            list_user = gson.fromJson(temp, userListType);
            Log.d("API.GET.USERS", temp);
            for(var i : list_user){
                list_user_adapter.add(new p_user_item(R.drawable.ic_launcher_foreground,i.FullName,i.Email));
            }
            runOnUiThread(()->{
                adapter = new UserAdapter(this.getLayoutInflater(), list_user_adapter);
                recyclerView.setAdapter(adapter);
            });
        }).start();

    }

}
