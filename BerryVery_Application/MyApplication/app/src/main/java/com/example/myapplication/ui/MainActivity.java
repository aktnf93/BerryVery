package com.example.myapplication.ui;

import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.fragment.app.Fragment;

import com.example.myapplication.R;


public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        // 화면 객체 생성
        super.onCreate(savedInstanceState);
        // 화면 객체에 activity_main.xml 로 화면 구현
        // R = com.example.myapplication.R = res 폴더
        setContentView(R.layout.activity_main);

        if (getSupportActionBar() != null) {
            getSupportActionBar().hide();
        }
    }

    public void onBtnMonitoringView(View v) {
        getSupportFragmentManager()
                .beginTransaction()
                .replace(R.id.main_container, new MonitoringFragment())
                .commit();

        Toast.makeText(this, "모니터링 화면", Toast.LENGTH_SHORT).show();
    }

    public void onBtnControlView(View v) {
        getSupportFragmentManager()
                .beginTransaction()
                .replace(R.id.main_container, new ControlFragment())
                .commit();

        Toast.makeText(this, "컨트롤 화면", Toast.LENGTH_SHORT).show();
    }
}