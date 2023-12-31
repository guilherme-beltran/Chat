import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ChatService } from '../services/chat.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  userForm: FormGroup = new FormGroup({});
  submitted: boolean = false;
  apiErrorMessages: string[] = [];
  openChat = false;

  constructor(private formBuilder: FormBuilder, private chatServices: ChatService) {    
    
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(){
    this.userForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(30)]]
    })
  }

  submitForm(){
    this.submitted = true;
    this.apiErrorMessages = [];

    if(this.userForm.valid){
      this.chatServices.registerUser(this.userForm.value).subscribe({
        next: () => {
          this.chatServices.myName = this.userForm.get('name')?.value;
          this.openChat = true;
          this.userForm.reset();
          this.submitted = false;
        },
        error: error => {
          if(typeof(error.error) !== 'object'){
            this.apiErrorMessages.push(error.error);
          }
        }
      })
    }
    
  }

  closeChat(){
    this.openChat = false;
  }

}
