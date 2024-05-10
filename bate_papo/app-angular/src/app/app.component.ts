import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { error } from 'console';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  private hubConnection : HubConnection;
  mensagens:string[] = []
  novamensagem:string = "";
  title = 'app-angular';

  constructor(){
    this.hubConnection = new HubConnectionBuilder()
      .withUrl("http://localhost:5245/chat")
      .build()

      this.hubConnection.start()
      .then(() => console.log('SignalR Connect'))
      .catch(err => console.log('Erro ao Conectar', err));

      this.hubConnection.on("ReceberMensagem", (mensagem:string) => {
        console.log(`Funfou, mensagem recebida ${mensagem}`);
        this.mensagens.push(mensagem)
      })
  }

  //Método envio mensagem
  envioMensagem(){
    this.hubConnection.invoke('EnviarMensagem', this.novamensagem) //esse 'EnviarMensagem' é o nome do método que está no back
    .catch(err => console.error(err));

    this.novamensagem = ""
  }
}
