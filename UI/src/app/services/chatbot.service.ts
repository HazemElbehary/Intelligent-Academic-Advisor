// src/app/services/chatbot.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ChatResponse {
  answer: string;
}

@Injectable({ providedIn: 'root' })
export class ChatbotService {
  private baseUrl = 'http://127.0.0.1:5000/chatbot/api';

  constructor(private http: HttpClient) { }

  ask(question: string): Observable<ChatResponse> {
    var x =  this.http.post<ChatResponse>(`${this.baseUrl}/ask`, { question });
    console.log(x);
    return x;
  }
}
