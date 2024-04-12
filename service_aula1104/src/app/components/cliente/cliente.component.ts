import { CommonModule } from '@angular/common';
import { Cliente } from '../../interfaces/cliente';
import { ClienteService } from './../../services/cliente.service';
import { Component } from '@angular/core';



@Component({
  selector: 'app-cliente',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cliente.component.html',
  styleUrl: './cliente.component.css'
})
export class ClienteComponent {
  clientes:Cliente[]=[]
  constructor(private clienteService:ClienteService){
  }

  listar():void{
    this.clientes = this.clienteService.listar()//fez uma lista cliente para recebe as infos da api e o front usa essa lista cliente
  }

  ngOnInit():void{//ngOnInit() --> quando inicia a página ele chama esse método listar desse clientCoponent
    this.listar()
  }
}
