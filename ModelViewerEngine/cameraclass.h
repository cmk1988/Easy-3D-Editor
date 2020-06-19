#ifndef _CAMERACLASS_H_
#define _CAMERACLASS_H_

#include <d3dx10math.h>

# define M_PI           3.14159265358979323846  /* pi */

class CameraClass
{
public:
	CameraClass();
	CameraClass(const CameraClass&);
	~CameraClass();

	void SetPosition(float, float, float);
	void SetRotation(float, float, float);
	void MoveInDirection(float);

	D3DXVECTOR3 GetPosition();
	D3DXVECTOR3 GetRotation();

	void Render();
	void GetViewMatrix(D3DXMATRIX&);
	void GetNullMatrix(D3DXMATRIX&);

	static D3DXMATRIX GetIdentitymatrix()
	{
		D3DXMATRIX mtx;

		mtx._11 = 1.0f;
		mtx._21 = 0.0f;
		mtx._31 = 0.0f;
		mtx._41 = 0.0f;

		mtx._12 = 0.0f;
		mtx._22 = 1.0f;
		mtx._32 = 0.0f;
		mtx._42 = 0.0f;

		mtx._13 = 0.0f;
		mtx._23 = 0.0f;
		mtx._33 = 1.0f;
		mtx._43 = 0.0f;

		mtx._14 = 0.0f;
		mtx._24 = 0.0f;
		mtx._34 = 0.0f;
		mtx._44 = 1.0f;

		return mtx;
	}

private:
	float m_positionX, m_positionY, m_positionZ;
	float m_rotationX, m_rotationY, m_rotationZ;
	D3DXMATRIX m_viewMatrix;
	D3DXMATRIX m_null;
};

#endif