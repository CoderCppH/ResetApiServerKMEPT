package com.example.myapplication.UI.FrindaListFragment;

import android.os.Bundle;

import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;

import com.example.myapplication.LIST_USER.UserAdapter;
import com.example.myapplication.LIST_USER.p_user_item;
import com.example.myapplication.R;

import java.util.ArrayList;
import java.util.List;

public class FrindaListFragment extends Fragment {

    private RecyclerView recyclerView;
    private UserAdapter adapter;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_frinda_list, container, false);
        recyclerView = view.findViewById(R.id.recycler_view);
        recyclerView.setLayoutManager(new LinearLayoutManager(getContext()));
        adapter = new UserAdapter(inflater, getData());
        recyclerView.setAdapter(adapter);
        Init(view);
        return view;
    }

    private void Init(View view) {
        Button btn = view.findViewById(R.id.btn_add_friend);
        btn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                p_user_item item = new p_user_item(R.drawable.ic_launcher_foreground, "Pargev", "coder.cpp.h@gmail.com");
                adapter.AddItem(item);
            }
        });
    }
    private List<p_user_item> getData() {
        ArrayList<p_user_item> list = new ArrayList<p_user_item>();
        p_user_item item = new p_user_item(R.drawable.ic_launcher_foreground, "admin", "Anton234@gmail.com");
        list.add(item);
        return list;
    }
}