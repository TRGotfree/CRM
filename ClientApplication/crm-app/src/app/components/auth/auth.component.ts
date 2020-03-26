import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../services/auth.service';
import { User } from '../../models/user';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent implements OnInit {

  constructor(private router: Router, private snackBar: MatSnackBar, private authService: AuthService) { }

  login: string;
  password: string;

  ngOnInit() {
  }

  authUser() {
    try {

      if (!this.login) {
        this.snackBar.open('Укажите логин!', 'OK', { duration: 3000 });
        return;
      }

      if (!this.password) {
        this.snackBar.open('Укажите пароль!', 'OK', { duration: 3000 });
        return;
      }

      const user: User = { login: this.login, password: this.password, name: '', roleId: 0, roleName: '' };
      this.authService.checkCredentials(user).subscribe((data: { token: '', user: User }) => {

      sessionStorage.setItem('jwt', `Bearer ${data.token}`);
      sessionStorage.setItem('user', JSON.stringify(data.user));
      this.router.navigate(['/home']);

      }, error => {
        if (error.status !== 401 && error.status !== 403) {
          this.snackBar.open(`${error.status} Сервер вернул ошибку!`, 'OK', { duration: 3000 });
        } else {
          this.snackBar.open(`${error.status} Неверный логин или пароль!`, 'OK', { duration: 3000 });
        }
      });

    } catch (error) {
      this.snackBar.open('Не удаётся проверить логин и пароль из-за непредвиденной ошибки!', 'OK', { duration: 3000 });
    }
  }

}
