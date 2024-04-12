import { Injectable } from '@angular/core';
import { Cliente } from '../interfaces/cliente';

@Injectable({ //permite ser injetado
  providedIn: 'root'
})
export class ClienteService {

  constructor() { }

  //criar lista na interface clientes
  clientes:Cliente[] =[
    {id: "sadsadgre", nomeCliente:"fernandi"},
    {id: "sadsadgre", nomeCliente:"fernandi", telefone:"584354"}
  ];

  listar():Cliente[]{
    return this.clientes;
  }

  remover(id:string){
    const cliente = this.clientes.find(c=> c.id == id)
    if (cliente) {
      const index = this.clientes.indexOf(cliente)
      this.clientes.splice(index,1)//(index,1)
    }
  }

  adicionae(cliente:Cliente){
    this.clientes.push(cliente)
  }
}
