// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function () {

    //Пользуемся методом объекта document.
    //querySelectorAll - метод. Позволяет передовать селекторы в формате css и получать DOM элементы, которые соответствуют данным селекторам.
    document.querySelectorAll('.FW').forEach(function (tabsLink) {
        //addEventListener - метод, который позволяет вызывать функцию, при появлении какого-нибудь события
        tabsLink.addEventListener('click', function (event) {
            //Значение переменной path берется из события клика
            //currentTarget - html элемент в который был совершен клик
            //dataset - набор data атрибутов. Атрибуты, которые начинаются в html с "data-", попатают в специальный объект dataset
            const path = event.currentTarget.dataset.path

            //выбираем все эл-ты с классом .tab-content
            document.querySelectorAll('.FefuPage_variant').forEach(function (tabContent) {
                //у каждого таба удаляем класс tab-content-active
                tabContent.classList.remove('FefuPage_variant_active')
            })
            //после этого у нас нет ни одного активного таба, и нам нужно показать тот, который хотел пользователь при клике. Выбираем html элемент по атрибуту, на это указывают []. Атрибут data-target должен быть равен значению {path}
            document.querySelector(`[data-target="${path}"]`).classList.add('FefuPage_variant_active')

            document.querySelectorAll('.FW').forEach(function (tabContent) {
                //у каждого таба удаляем класс tab-content-active
                tabContent.classList.remove('FefuPage_week_fr_l')

            })

            document.querySelector(`[data-path="${path}"]`).classList.add('FefuPage_week_fr_l')
        })
    })
     
})