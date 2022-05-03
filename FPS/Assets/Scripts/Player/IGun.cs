using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

 public interface IGun
 {
    public void OnShoot();
    public void SetDatas(Camera fpsCam);
 }
